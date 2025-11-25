using GenericRepoWebApi.Entity;

namespace GenericRepoWebApi.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductByName(string productName);
    }
}
