using DAL.Entities;
using DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        public GenericRepository<Blog> BlogRepository { get; }
        public GenericRepository<Article> ArticleRepository { get; }
        public GenericRepository<Comment> CommentRepository { get; }
        public GenericRepository<Teg> TegRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
