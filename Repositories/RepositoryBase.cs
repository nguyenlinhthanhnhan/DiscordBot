using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repositories
{
    public class RepositoryBase<T>:IRepositoryBase<T> where T: class
    {
        private protected DataContext _dataContext;
        public RepositoryBase(DataContext dataContext) => _dataContext = dataContext;

        public void Create(T entity) => _dataContext.Set<T>().Add(entity);

        public void Delete(T entity) => _dataContext.Set<T>().Remove(entity);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
            => !trackChanges ? _dataContext.Set<T>().Where(expression).AsNoTracking() : _dataContext.Set<T>().Where(expression);

        public IQueryable<T> FindAll(bool trackChanges) 
            => !trackChanges ? _dataContext.Set<T>().AsNoTracking() : _dataContext.Set<T>();
        public void Update(T entity) => _dataContext.Set<T>().Remove(entity);
        public void Detach(T entity) => _dataContext.Entry(entity).State = EntityState.Detached;
    }
}
