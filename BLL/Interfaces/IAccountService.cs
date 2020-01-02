using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> RegisterRegularUser(UserDTO userDTO);
        Task<UserDTO> RegisterModerator(UserDTO userDTO);
        Task<IEnumerable<UserDTO>> GetAllRegularUsers();
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(string id, string token);
        Task<bool> DeleteUser(string id, string token);
    }
}
