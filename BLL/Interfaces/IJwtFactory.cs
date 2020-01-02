using DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IJwtFactory
    {
        JwtSecurityToken GenerateDecodedToken(string token);
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        Task<ClaimsIdentity> GenerateClaimsIdentity(User user);
        string GetUserIdClaim(string token);
        string GetUserRoleClaim(string token);
    }
}
