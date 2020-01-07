using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBlogService
    {
        Task<BlogDTO> CreateBlog(BlogDTO blog, string token);
        void DeleteBlog(int id, string token);
        void UpdateBlogName(int id, BlogDTO blog, string token);
        BlogDTO GetBlogById(int id);
        IEnumerable<BlogDTO> GetAllBlogs();
        IEnumerable<ArticleDTO> GetAllArticlesByBlogId(int id);
    }
}
