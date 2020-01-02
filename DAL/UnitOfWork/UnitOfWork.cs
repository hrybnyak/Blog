using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork:IDisposable, IUnitOfWork
    {
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly ApplicationDbContext _context;
        private GenericRepository<Blog> _blogRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Comment> _commentRepository;
        private GenericRepository<Teg> _tegRepository;
        public GenericRepository<Blog> BlogRepository
        {
            get
            {

                if (this._blogRepository == null)
                {
                    this._blogRepository = new GenericRepository<Blog>(_context);
                }
                return _blogRepository;
            }
        }
        public GenericRepository<Article> ArticleRepository
        {
            get
            {

                if (this._articleRepository == null)
                {
                    this._articleRepository = new GenericRepository<Article>(_context);
                }
                return _articleRepository;
            }
        }
        public GenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this._commentRepository == null)
                {
                    this._commentRepository = new GenericRepository<Comment>(_context);
                }
                return _commentRepository;
            }

        }
        public GenericRepository<Teg> TegRepository
        {
            get
            {

                if (this._tegRepository == null)
                {
                    this._tegRepository = new GenericRepository<Teg>(_context);
                }
                return _tegRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
