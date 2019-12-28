using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private UserMapper _userMapper;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
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

        public async Task<UserDTO> RegisterRegularUser(UserDTO userDTO)
        {
            if (await _userManager.FindByEmailAsync(userDTO.Email) != null) throw new EmailIsAlreadyTakenException();
            if (await _userManager.FindByNameAsync(userDTO.UserName) != null) throw new UsernameIsAlreadyTakenException();
            if (!CheckPassword(userDTO.Password)) throw new InvalidPasswordException();
            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "RegularUser");
            }
            return UserMapper.Map(user);
        }

        public bool CheckPassword(string password)
        {
            if (password.Length < 6) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            return true;
        }
    }
}
