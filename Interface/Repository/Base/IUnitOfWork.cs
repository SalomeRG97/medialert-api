using Microsoft.EntityFrameworkCore.Storage;

namespace Interface.Repository.Base
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();
        Task Commit();
        void Dispose();
    }
}