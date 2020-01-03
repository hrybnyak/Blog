using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<User> _userManager;

        private CommentMapper _commentMapper;
        
        private CommentMapper CommentMapper
        {
            get
            {
                if (_commentMapper == null)
                {
                    _commentMapper = new CommentMapper();
                }
                return _commentMapper;
            }
        }

        public CommentService(IUnitOfWork unitOfWork, IJwtFactory jwtFactory, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtFactory = jwtFactory;
            _userManager = userManager;
        }

        public async Task<CommentDTO> AddComment(CommentDTO comment, string token)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (comment.Content == null) throw new ArgumentNullException(nameof(comment.Content));
            if (comment.ArticleId == null) throw new ArgumentNullException(nameof(comment.ArticleId));
            if (token == null) throw new ArgumentNullException(nameof(token));

            string userId = _jwtFactory.GetUserIdClaim(token);
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new ArgumentNullException(nameof(user));

            var commentEntity = CommentMapper.Map(comment);
            commentEntity.UserId = userId;
            commentEntity.LastUpdated = DateTime.Now;

            await _unitOfWork.CommentRepository.InsertAsync(commentEntity);

            var result = CommentMapper.Map(commentEntity);
            result.CreatorUsername = user.UserName;
            return result;
        }
        public void DeleteComment (CommentDTO comment, string token)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (comment.Id == null) throw new ArgumentNullException(nameof(comment.Id));
            
            var entity = _unitOfWork.CommentRepository.GetById(comment.Id.GetValueOrDefault());
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Comment wasn't found");
            string userId = _jwtFactory.GetUserIdClaim(token);
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (entity.UserId != userId)
            {
                string userRole = _jwtFactory.GetUserRoleClaim(token);
                if (userRole.CompareTo("Moderator") != 0) throw new NotEnoughtRightsException();
            }
            _unitOfWork.CommentRepository.Delete(entity);
            _unitOfWork.Save();
        }
        public void UpdateComment (CommentDTO comment, string token)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (comment.Id == null) throw new ArgumentNullException(nameof(comment.Id));

            var entity = _unitOfWork.CommentRepository.GetById(comment.Id.GetValueOrDefault());
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Comment wasn't found");
            string userId = _jwtFactory.GetUserIdClaim(token);
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (entity.UserId != userId) throw new NotEnoughtRightsException();

            if (comment.Content != null) entity.Content = comment.Content;
            entity.LastUpdated = DateTime.Now;
            _unitOfWork.CommentRepository.Update(entity);
            _unitOfWork.Save();
        }

        public async Task<CommentDTO> GetCommentById (int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            return CommentMapper.Map(comment);
        }

        public IEnumerable<CommentDTO> GetAllComments()
        {
            var comments = _unitOfWork.CommentRepository.Get();
            if (comments == null) throw new ArgumentNullException(nameof(comments));
            return CommentMapper.Map(comments);
        }
    }
}
