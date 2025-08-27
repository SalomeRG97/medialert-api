using Interface.Repository.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Model.Data;

namespace Repository.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MedsAppContext _context;
        //private ITaskRepository _taskRepository;

        public UnitOfWork(MedsAppContext context)
        {
            _context = context;
        }

        //public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(_context);


        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
