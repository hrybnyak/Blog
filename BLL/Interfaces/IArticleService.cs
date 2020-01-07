using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDTO> CreateArticle(ArticleDTO article, string token);
        void DeleteArticle(int id, string token);
        void UpdateArticle(int id, ArticleDTO article, string token);
        ArticleDTO GetArticleById(int id);
        ICollection<TegDTO> GetTegsByArticleId(int id);
        ICollection<CommentDTO> GetCommentsByArticleId(int id);
        IEnumerable<ArticleDTO> GetArticlesWihtTextFilter(string filter);
        IEnumerable<ArticleDTO> GetAllArticles();
        IEnumerable<ArticleDTO> GetArticlesWithTegFilter(IEnumerable<TegDTO> tegs);
        IEnumerable<ArticleDTO> GetArticlesWithTegFilter(string tegs);
    }
}
