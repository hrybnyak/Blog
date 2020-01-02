using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtFactory _jwtFactory;
        private ArticleMapper _articleMapper;
        private TegMapper _tegMapper;
        private CommentMapper _commentMapper;

        private ArticleMapper ArticleMapper
        {
            get
            {
                if (_articleMapper == null)
                {
                    _articleMapper = new ArticleMapper();
                }
                return _articleMapper;
            }
        }
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
        private TegMapper TegMapper
        {
            get
            {
                if(_tegMapper == null)
                {
                    _tegMapper = new TegMapper();
                }
                return _tegMapper;
            }
        }
        public ArticleService(IUnitOfWork unitOfWork, IJwtFactory jwtFactory)
        {
            _unitOfWork = unitOfWork;
            _jwtFactory = jwtFactory;
        }

        private async Task AddTegs(Article articleEntity, ArticleDTO article)
        {
            if (article.Tegs != null && article.Tegs.Count > 0)
            {
                foreach (TegDTO teg in article.Tegs)
                {
                    var tegEntity = _unitOfWork.TegRepository.Get(t => t.Name == teg.Name).FirstOrDefault();
                    if (tegEntity == null)
                    {
                        tegEntity = TegMapper.Map(teg);
                        await _unitOfWork.TegRepository.InsertAsync(tegEntity);
                        await _unitOfWork.SaveAsync();
                    }
                    var connection = new ArticleTeg
                    {
                        ArticleId = articleEntity.Id,
                        TegId = tegEntity.Id
                    };
                    tegEntity.ArticleTegs.Add(connection);
                    //articleEntity.ArticleTegs.Add(connection);
                    _unitOfWork.TegRepository.Update(tegEntity);
                    await _unitOfWork.SaveAsync();
                }
            }
        }

        public async Task<ArticleDTO> CreateArticle(ArticleDTO article, string token)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            if (article.Name == null) throw new ArgumentNullException(nameof(article.Name));
            if (article.Content == null) throw new ArgumentNullException(nameof(article.Content));
            if (article.BlogId == null) throw new ArgumentNullException(nameof(article.BlogId));

            string ownerId = _unitOfWork.BlogRepository.GetById(article.BlogId.GetValueOrDefault()).OwnerId;
            if (ownerId.CompareTo(_jwtFactory.GetUserIdClaim(token)) != 0) throw new NotEnoughtRightsException();
            var articleEntity = ArticleMapper.Map(article);
            articleEntity.LastUpdate = DateTime.Now;
            await _unitOfWork.ArticleRepository.InsertAsync(articleEntity);
            await _unitOfWork.SaveAsync();
            var result = ArticleMapper.Map(articleEntity);
            result.Tegs = article.Tegs;
            return result;
        }

        public void DeleteArticle(ArticleDTO article, string token)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            if (article.BlogId == null) throw new ArgumentNullException(nameof(article));
            if (article.Id == null) throw new ArgumentNullException(nameof(article));
            string ownerId = _unitOfWork.BlogRepository.GetById(article.BlogId.GetValueOrDefault()).OwnerId;
            if (ownerId.CompareTo(_jwtFactory.GetUserIdClaim(token)) != 0)
            {
                if (_jwtFactory.GetUserRoleClaim(token).CompareTo("Moderator") != 0 ) throw new NotEnoughtRightsException();
            }
            var entity = _unitOfWork.ArticleRepository.GetById(article.Id.GetValueOrDefault());
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _unitOfWork.ArticleRepository.Delete(entity);
            _unitOfWork.Save();
         }

        public void UpdateArticle(ArticleDTO article, string token)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            if (article.BlogId == null) throw new ArgumentNullException(nameof(article));
            if (article.Id == null) throw new ArgumentNullException(nameof(article));
            string ownerId = _unitOfWork.BlogRepository.GetById(article.BlogId.GetValueOrDefault()).OwnerId;
            if (ownerId.CompareTo(_jwtFactory.GetUserIdClaim(token)) != 0) throw new NotEnoughtRightsException();
            var entity = _unitOfWork.ArticleRepository.GetById(article.Id.GetValueOrDefault());
            entity.Name = article.Name;
            entity.Content = article.Content;
            entity.LastUpdate = DateTime.Now;
            _unitOfWork.ArticleRepository.Update(entity);
            _unitOfWork.Save();
        }

        public ArticleDTO GetArticleById(int id)
        {
            var article = _unitOfWork.ArticleRepository.Get(a => a.Id == id, includeProperties: "Comments,ArticleTegs").FirstOrDefault();
            if (article == null) throw new ArgumentNullException(nameof(article));
            var result = ArticleMapper.Map(article);
            if (article.Comments!=null && article.Comments.Count > 0) 
            result.Comments = CommentMapper.Map(article.Comments).ToList();
            if (article.ArticleTegs != null && article.ArticleTegs.Count > 0)
            {
                foreach (ArticleTeg teg in article.ArticleTegs)
                    result.Tegs.Add(TegMapper.Map(_unitOfWork.TegRepository.GetById(teg.TegId)));
            }
            return result;   
        }
    }
}
