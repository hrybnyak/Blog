using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private UserMapper _userMapper;
        private IJwtFactory _jwtFactory;
        private IUnitOfWork _unitOfWork;
        private BlogMapper _blogMapper;
        private CommentMapper _commentMapper;
        public AccountService(UserManager<User> userManager, IJwtFactory jwtFactory, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _unitOfWork = unitOfWork;
        }

        private BlogMapper BlogMapper
        {
            get
            {
                if(_blogMapper == null)
                {
                    _blogMapper = new BlogMapper();
                }
                return _blogMapper;
            }
        }

        private UserMapper UserMapper
        {
            get
            {
                if (_userMapper == null)
                {
                    _userMapper = new UserMapper();
                }
                return _userMapper;
            }
        }
        private CommentMapper CommentMapper
        {
            get
            {
                if (_commentMapper == null)
                {
                    _commentMapper = new CommentMapper();
                }
                return _commentMapper;
            }
        }
        private async Task<User> CreateUser(UserDTO userDTO)
        {
            if (await _userManager.FindByEmailAsync(userDTO.Email) != null) throw new EmailIsAlreadyTakenException();
            if (await _userManager.FindByNameAsync(userDTO.UserName) != null) throw new NameIsAlreadyTakenException();
            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded) return user;
            else return null;
        }
        public async Task<UserDTO> RegisterRegularUser(UserDTO userDTO)
        {
            var user = await CreateUser(userDTO);
            if (user == null) throw new ArgumentNullException(nameof(user),"Couldn't create user");
            else {
                await _userManager.AddToRoleAsync(user, "RegularUser");
            }
            return UserMapper.Map(user);
        }
        public async Task<UserDTO> RegisterModerator(UserDTO userDTO)
        {
            var user = await CreateUser(userDTO);
            if (user == null) throw new ArgumentNullException(nameof(user), "Couldn't create user");
            else
            {
                await _userManager.AddToRoleAsync(user, "Moderator");
            }
            return UserMapper.Map(user);
        } 
        public async Task<IEnumerable<UserDTO>> GetAllRegularUsers()
        {
            return UserMapper.Map((await _userManager.GetUsersInRoleAsync("RegularUser")).ToList());
        }
        public async Task<IEnumerable<UserDTO>> GetAllModerators()
        {
            return UserMapper.Map((await _userManager.GetUsersInRoleAsync("Moderator")).ToList());
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            List<UserDTO> users = (await GetAllRegularUsers()).ToList();
            users.AddRange(UserMapper.Map((await _userManager.GetUsersInRoleAsync("Moderator")).ToList()));
            return users;
        }
        public async Task<UserDTO> GetUserById(string id, string token)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (token == null) throw new ArgumentNullException(nameof(token));
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new ArgumentNullException(nameof(user), "Couldn't find user with this id");

            string claimsId = _jwtFactory.GetUserIdClaim(token);
            if (claimsId == id) return UserMapper.Map(user);
            string claimsRole = _jwtFactory.GetUserRoleClaim(token);
            if (claimsRole == "Moderator")
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(r => r == "Moderator" || r == "Admin")) throw new NotEnoughtRightsException();
                else return UserMapper.Map(user);
            }
            else if (claimsRole == "Admin")
            {
                return UserMapper.Map(user);
            }
            else throw new NotEnoughtRightsException();
        }
        public async Task<bool> DeleteUser(string id, string token)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (token == null) throw new ArgumentNullException(nameof(token));
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new ArgumentNullException(nameof(user), "Couldn't find user with this id");

            string claimsId = _jwtFactory.GetUserIdClaim(token);
            if (claimsId == id) return (await _userManager.DeleteAsync(user)).Succeeded;
            string claimsRole = _jwtFactory.GetUserRoleClaim(token);
            if (claimsRole == "Moderator")
            {
                throw new NotEnoughtRightsException();
            }
            else if (claimsRole == "Admin") return (await _userManager.DeleteAsync(user)).Succeeded;
            else throw new NotEnoughtRightsException();
        }

        public async Task UpdateUser (string id, UserDTO user, string token)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserName == null && user.Email == null) throw new ArgumentNullException(nameof(user));

            string claimsId = _jwtFactory.GetUserIdClaim(token);

            var userEntity = await _userManager.FindByIdAsync(id);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id");

            if (claimsId == id)
            {
                if (user.UserName != null && userEntity.UserName.CompareTo(user.UserName) != 0)
                {
                    var checkIfNameIsTaken = await _userManager.FindByNameAsync(user.UserName);
                    if (checkIfNameIsTaken != null) throw new NameIsAlreadyTakenException();
                    userEntity.UserName = user.UserName;
                }
                else if (user.Email != null && userEntity.Email.CompareTo(user.Email) != 0)
                {
                    var checkIfNameIsTaken = await _userManager.FindByEmailAsync(user.Email);
                    if (checkIfNameIsTaken != null) throw new NameIsAlreadyTakenException();
                    userEntity.Email = user.Email;
                }
                await _userManager.UpdateAsync(userEntity);
            }
            else throw new NotEnoughtRightsException();
        }
        public async Task ChangePassword(string id, PasswordDTO password, string token)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (password.NewPassword == null) throw new ArgumentNullException(nameof(password.NewPassword));
            if (password.OldPassword == null) throw new ArgumentNullException(nameof(password.OldPassword));

            string claimsId = _jwtFactory.GetUserIdClaim(token);
            var userEntity = await _userManager.FindByIdAsync(id);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id");

            if (claimsId == id)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(userEntity, password.OldPassword);
                if (checkPassword == false) throw new ArgumentException(nameof(password));
                else await _userManager.ChangePasswordAsync(userEntity, password.OldPassword, password.NewPassword);
            }
            else throw new NotEnoughtRightsException();
        }

        public async Task<IEnumerable<BlogDTO>> GetAllBlogsByUserId(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id");
            var blogs = _unitOfWork.BlogRepository.Get(b => b.OwnerId == id);
            return BlogMapper.Map(blogs);
        }

        public async Task<IEnumerable<CommentDTO>> GetAllCommentsByUserId(string id)
        {
            if (id == null) throw new ArgumentNullException();
            var userEntity = await _userManager.FindByIdAsync(id);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id");
            var comments = _unitOfWork.CommentRepository.Get(c => c.UserId == id);
            return CommentMapper.Map(comments);
        }

        public static string SaveImage(IFormFile image, IWebHostEnvironment _environment)
        {
            var randomName = $"{Guid.NewGuid()}." + image.ContentType.Substring(6);
            string path = _environment.WebRootPath + "\\Upload\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream fileStream = System.IO.File.Create(path + randomName))
            {
                image.CopyTo(fileStream);
                fileStream.Flush();
                return "\\Upload\\" + randomName;
            }
        }
    }
}
