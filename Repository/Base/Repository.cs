using Interface.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using System.Linq.Expressions;

namespace Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MedsAppContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(MedsAppContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
                               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                               string includeString = null,
                               bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();


            return await query.ToListAsync();
        }


        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  List<Expression<Func<T, object>>> includes = null,
                                  bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).FirstOrDefaultAsync();

            return await query.FirstOrDefaultAsync();

        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
        }
        public async Task<T?> GetOne(Expression<Func<T, bool>> funcion)
        {
            return await _context.Set<T>().AsNoTracking().Where(funcion).FirstOrDefaultAsync();
        }

        public async Task<T?> GetOneT(Expression<Func<T, bool>> funcion)
        {
            return await _context.Set<T>().Where(funcion).FirstOrDefaultAsync();
        }

        public async Task<T?> GetOneNoTracking(Expression<Func<T, bool>> funcion)
        {
            return await _context.Set<T>().Where(funcion).FirstOrDefaultAsync();
        }
        public async Task AddRange(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void SaveChangesAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}
