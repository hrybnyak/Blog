using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDTO> CreateArticle(ArticleDTO article, string token);
        void DeleteArticle(ArticleDTO article, string token);
        void UpdateArticle(ArticleDTO article, string token);
        ArticleDTO GetArticleById(int id);
        ICollection<TegDTO> GetTegsByArticleId(int id);
        ICollection<CommentDTO> GetCommentsByArticleId(int id);
        IEnumerable<ArticleDTO> GetArticlesWihtTextFilter(string filter);
        IEnumerable<ArticleDTO> GetAllArticles();
    }
}
