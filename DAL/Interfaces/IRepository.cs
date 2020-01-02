using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");
        T GetById(int id);
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<T> GetByIdAsync(int id);
        Task InsertAsync(T entity);
    }
}
