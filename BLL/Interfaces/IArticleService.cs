using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
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
