using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBlogService
    {
        Task<BlogDTO> CreateBlog(BlogDTO blog, string token);
        void DeleteBlog(BlogDTO blog, string token);
        void UpdateBlogName(BlogDTO blog, string token);
        BlogDTO GetBlogById(int id);
        IEnumerable<BlogDTO> GetAllBlogs();
    }
}
