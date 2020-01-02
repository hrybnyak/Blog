using BLL.DTO;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDTO> CreateArticle(ArticleDTO article, string token);
        void DeleteArticle(ArticleDTO article, string token);
        void UpdateArticle(ArticleDTO article, string token);
        ArticleDTO GetArticleById(int id);
    }
}
