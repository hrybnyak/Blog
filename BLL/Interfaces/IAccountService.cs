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
        Task<bool> DeleteUser(string id, string token);
        Task UpdateUser(string id, UserDTO user, string token);
        Task ChangePassword(string id, PasswordDTO password, string token);
        Task<IEnumerable<BlogDTO>> GetAllBlogsByUserId(string id);
        Task<IEnumerable<CommentDTO>> GetAllCommentsByUserId(string id);
    }
}
