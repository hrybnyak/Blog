using DAL.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IJwtFactory
    {
        JwtSecurityToken GenerateDecodedToken(string token);
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        Task<ClaimsIdentity> GenerateClaimsIdentity(User user);
    }
}
