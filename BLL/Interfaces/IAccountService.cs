using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> RegisterRegularUser(UserDTO userDTO);
        Task<UserDTO> RegisterModerator(UserDTO userDTO);
        Task<IEnumerable<UserDTO>> GetAllRegularUsers();
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<IEnumerable<UserDTO>> GetAllModerators();
        Task<UserDTO> GetUserById(string id, string token);
        Task<UserDTO> GetUserById(string token);
        Task<bool> DeleteUser(string token);
        Task<bool> DeleteModerator(string id, string token);
        Task UpdateUsername(UserDTO user, string token);
        Task ChangePassword(PasswordDTO password, string token);
    }
}
