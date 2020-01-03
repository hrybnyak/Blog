using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private UserMapper _userMapper;
        private IJwtFactory _jwtFactory;

        public AccountService(UserManager<User> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
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
        public async Task<UserDTO> GetUserById(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            string claimsId = _jwtFactory.GetUserIdClaim(token);
            var user = await _userManager.FindByIdAsync(claimsId);
            if (user == null) throw new ArgumentNullException(nameof(user), "Couldn't find user with this id");
             return UserMapper.Map(user);
        }
        public async Task<bool> DeleteUser(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            string claimsId = _jwtFactory.GetUserIdClaim(token);
            var userEntity = await _userManager.FindByIdAsync(claimsId);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id claim");
            var result = await _userManager.DeleteAsync(userEntity);
            return result.Succeeded;
        }
        public async Task<bool> DeleteModerator(string id, string token)
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
        public async Task UpdateUsername (UserDTO user, string token)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserName == null) throw new ArgumentNullException(nameof(user.Id));

            string claimsId = _jwtFactory.GetUserIdClaim(token);

            var userEntity = await _userManager.FindByIdAsync(claimsId);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id claim");
            var checkIfNameIsTaken = await _userManager.FindByNameAsync(user.UserName);
            if (checkIfNameIsTaken != null) throw new NameIsAlreadyTakenException();
            userEntity.UserName = user.UserName;
        }
        public async Task ChangePassword(PasswordDTO password, string token)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (password.NewPassword == null) throw new ArgumentNullException(nameof(password.NewPassword));
            if (password.OldPassword == null) throw new ArgumentNullException(nameof(password.OldPassword));

            string claimsId = _jwtFactory.GetUserIdClaim(token);
            var userEntity = await _userManager.FindByIdAsync(claimsId);
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity), "Couldn't find user with this id claim");
            bool checkPassword = await _userManager.CheckPasswordAsync(userEntity, password.OldPassword);
            if (checkPassword == false) throw new ArgumentException(nameof(password));
            else await _userManager.ChangePasswordAsync(userEntity, password.OldPassword, password.NewPassword);
        }

    }
}
