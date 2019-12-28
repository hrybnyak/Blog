using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        public readonly UserManager<User> _userManager;
        public readonly IJwtFactory _jwtFactory;
        public readonly JwtIssuerOptions _jwtOptions;

        public AuthService(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException();
            if (user.UserName == null || user.Password == null) throw new ArgumentNullException();
            var userToVerify = await _userManager.FindByNameAsync(user.UserName);
            if (userToVerify == null)
            {
                userToVerify = await _userManager.FindByEmailAsync(user.UserName);
                if (userToVerify == null)
                {
                    throw new WrongCredentialsException();
                }
            }
            if (await _userManager.CheckPasswordAsync(userToVerify, user.Password))
            {
                return await _jwtFactory.GenerateClaimsIdentity(userToVerify);
            }
            else
            {
                throw new WrongCredentialsException();
            }
        }

        public async Task<object> Authenticate(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException();
            var identity = await GetClaimsIdentity(user);
            if (identity == null) throw new ArgumentNullException();
            string token = await _jwtFactory.GenerateEncodedToken(user.UserName, identity);
            if (token == null) throw new ArgumentNullException();
            return new
            {
                id = identity.Name,
                auth_token = token,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };
        }
    }
}
