using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDTO> GetCommentById(int id);
        void UpdateComment(CommentDTO comment, string token);
        void DeleteComment(CommentDTO comment, string token);
        Task<CommentDTO> AddComment(CommentDTO comment, string token);
        IEnumerable<CommentDTO> GetAllComments();

    }
}
