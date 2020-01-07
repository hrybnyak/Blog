using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDTO> GetCommentById(int id);
        void UpdateComment(int id, CommentDTO comment, string token);
        void DeleteComment(int id, string token);
        Task<CommentDTO> AddComment(CommentDTO comment, string token);
        IEnumerable<CommentDTO> GetAllComments();

    }
}
