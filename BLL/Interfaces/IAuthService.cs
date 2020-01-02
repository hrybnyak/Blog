using BLL.DTO;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthService
    {
        Task<object> Authenticate(UserDTO user);
    }
}
