using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
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
        private UserManager<User> _userManager;
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
        public ArticleService(IUnitOfWork unitOfWork, IJwtFactory jwtFactory, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtFactory = jwtFactory;
            _userManager = userManager;
        }

        private async Task AddTegs(Article articleEntity, ArticleDTO article)
        {
            if (article.Tegs != null && article.Tegs.Count > 0)
            {
                foreach (TegDTO teg in article.Tegs)
                {
                    var tegEntity = _unitOfWork.TegRepository.Get(t => t.Name == teg.Name, includeProperties: "ArticleTegs").FirstOrDefault();
                    if (tegEntity == null)
                    {
                        tegEntity = TegMapper.Map(teg);
                        tegEntity.ArticleTegs = new List<ArticleTeg>();
                        await _unitOfWork.TegRepository.InsertAsync(tegEntity);
                        await _unitOfWork.SaveAsync();
                    }
                    var connection = new ArticleTeg
                    {
                        ArticleId = articleEntity.Id,
                        TegId = tegEntity.Id
                    };
                    tegEntity.ArticleTegs.Add(connection);
                    _unitOfWork.TegRepository.Update(tegEntity);
                    await _unitOfWork.SaveAsync();
                }
            }
        }

        public IEnumerable<ArticleDTO> GetArticlesWithTegFilter(string tegs)
        {
            if (tegs == null) throw new ArgumentNullException(nameof(tegs));
            List<TegDTO> teg = new List<TegDTO>();
            string[] names = tegs.Split(',');
            foreach(string name in names)
            {
                teg.Add(new TegDTO { Name = name });
            }
            return GetArticlesWithTegFilter(teg);
        }

        public IEnumerable<ArticleDTO> GetArticlesWithTegFilter(IEnumerable<TegDTO> tegs)
        {
            IEnumerable<Article> articles = _unitOfWork.ArticleRepository.Get(includeProperties: "ArticleTegs");
            List<Article> result = new List<Article>();
            foreach (TegDTO teg in tegs)
            {
                Teg tegEntity;
                if (teg.Name != null)
                {
                    tegEntity = _unitOfWork.TegRepository.Get(t => t.Name == teg.Name).FirstOrDefault();
                    if (tegEntity == null) throw new ArgumentNullException(nameof(tegEntity));
                }
                else
                {
                    throw new ArgumentNullException(nameof(teg));
                }
                var filtered = articles.Where(a => a.ArticleTegs.Contains(a.ArticleTegs.Where(at=>at.ArticleId == a.Id && at.TegId == tegEntity.Id).FirstOrDefault()));
                foreach (Article article in filtered) {
                    if (!result.Contains(article))
                    {
                        result.Add(article);
                    }
                }
            }
            return ArticleMapper.Map(result);
        }
        public async Task<ArticleDTO> CreateArticle(ArticleDTO article, string token)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));
            if (article.Name == null) throw new ArgumentNullException(nameof(article.Name));
            if (article.Content == null) throw new ArgumentNullException(nameof(article.Content));

            string ownerId = _unitOfWork.BlogRepository.GetById(article.BlogId.GetValueOrDefault()).OwnerId;
            if (ownerId.CompareTo(_jwtFactory.GetUserIdClaim(token)) != 0) throw new NotEnoughtRightsException();
            var articleEntity = ArticleMapper.Map(article);
            articleEntity.LastUpdate = DateTime.Now;
            await _unitOfWork.ArticleRepository.InsertAsync(articleEntity);
            await _unitOfWork.SaveAsync();
            await AddTegs(articleEntity, article);
            var result = ArticleMapper.Map(articleEntity);
            if (articleEntity.ArticleTegs != null && articleEntity.ArticleTegs.Count > 0)
            {
                result.Tegs = new List<TegDTO>();
                foreach (ArticleTeg teg in articleEntity.ArticleTegs)
                    result.Tegs.Add(TegMapper.Map(_unitOfWork.TegRepository.GetById(teg.TegId)));
            }
            return result;
        }

        public void DeleteArticle(int id, string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            var entity = _unitOfWork.ArticleRepository.GetById(id);
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            string ownerId = _unitOfWork.BlogRepository.GetById(entity.BlogId).OwnerId;
            if (ownerId.CompareTo(_jwtFactory.GetUserIdClaim(token)) != 0)
            {
                if (_jwtFactory.GetUserRoleClaim(token).CompareTo("Moderator") != 0 ) throw new NotEnoughtRightsException();
            }
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _unitOfWork.ArticleRepository.Delete(entity);
            _unitOfWork.Save();
         }

        public void UpdateArticle(int id, ArticleDTO article, string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (article == null) throw new ArgumentNullException(nameof(article));
            var entity = _unitOfWork.ArticleRepository.GetById(id);
            if (entity == null) throw new ArgumentNullException(nameof(article));
            string ownerId = _unitOfWork.BlogRepository.GetById(entity.BlogId).OwnerId;
            if (ownerId.CompareTo(_jwtFactory.GetUserIdClaim(token)) != 0) throw new NotEnoughtRightsException();
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
            {
                result.Comments = new List<CommentDTO>();
                foreach (Comment comment in article.Comments)
                {
                    CommentDTO dto = CommentMapper.Map(comment);
                    dto.CreatorUsername = _userManager.FindByIdAsync(comment.UserId).Result.UserName;
                    result.Comments.Add(dto);
                }
            }

            
            result.AuthorId = _unitOfWork.BlogRepository.GetById(article.BlogId).OwnerId;
            result.AuthorUsername = _userManager.FindByIdAsync(result.AuthorId).Result.UserName;
            if (article.ArticleTegs != null && article.ArticleTegs.Count > 0)
            {
                result.Tegs = new List<TegDTO>();
                foreach (ArticleTeg teg in article.ArticleTegs)
                    result.Tegs.Add(TegMapper.Map(_unitOfWork.TegRepository.GetById(teg.TegId)));
            }
            return result;   
        }

        public ICollection<CommentDTO> GetCommentsByArticleId (int id)
        {
            var article = GetArticleById(id);
            return article.Comments;
        }

        public ICollection<TegDTO> GetTegsByArticleId (int id)
        {
            var article = GetArticleById(id);
            return article.Tegs;
        }

        public IEnumerable<ArticleDTO> GetArticlesWihtTextFilter(string filter)
        {
            var articles = _unitOfWork.ArticleRepository.Get(a => a.Content.Contains(filter) || a.Name.Contains(filter));
            if (articles == null) throw new ArgumentNullException(nameof(articles));
            return ArticleMapper.Map(articles);
        }

        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            var articles = _unitOfWork.ArticleRepository.Get();
            if (articles == null) throw new ArgumentNullException(nameof(articles));
            return ArticleMapper.Map(articles);
        }
    }
}
