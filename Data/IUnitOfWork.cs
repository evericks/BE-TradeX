using Data.Repositories.Interfaces;

namespace Data
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Dispose();
        Task<int> SaveChangesAsync();
    }
}
