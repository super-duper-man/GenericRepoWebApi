
using GenericRepoWebApi.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepoWebApi.Repository
{
    public class UnitOfWork(AppDbContext _dbContext) : IUnitOfWork
    {
        private readonly AppDbContext appDbContext = _dbContext;
        private IDbContextTransaction _transaction;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public IProductRepository productRepository => new ProductRepository(appDbContext);

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T))) {
                return _repositories[typeof(T)] as IRepository<T>;
            }

            var repository = new Repository<T>(appDbContext);
            _repositories.Add(typeof(T), repository);
            return repository;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await appDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null!;
            }
        }


        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
           Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    appDbContext.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
