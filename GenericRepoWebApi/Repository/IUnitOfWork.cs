namespace GenericRepoWebApi.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        public IProductRepository productRepository { get; }

        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

    }
}
