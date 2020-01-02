using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            if (!CheckPassword(userDTO.Password)) throw new InvalidPasswordException();
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

        public bool CheckPassword(string password)
        {
            if (password.Length < 6) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            return true;
        }
        public async Task<bool> DeleteUser(string id, string token)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (token == null) throw new ArgumentNullException(nameof(token));

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new ArgumentNullException(nameof(user), "Couldn't find user with this id");

            string claimsId = _jwtFactory.GetUserIdClaim(token);

            if (claimsId == id) { 
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            else throw new NotEnoughtRightsException();
        }
    }
}
