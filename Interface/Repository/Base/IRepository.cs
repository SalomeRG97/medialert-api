using System.Linq.Expressions;

namespace Interface.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRange(List<T> entity);
        void DeleteAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true);
        Task<T?> GetOne(Expression<Func<T, bool>> funcion);
        Task<T?> GetOneNoTracking(Expression<Func<T, bool>> funcion);
        Task<T?> GetOneT(Expression<Func<T, bool>> funcion);
        void SaveChangesAsync();
        Task UpdateAsync(T entity);
    }
}