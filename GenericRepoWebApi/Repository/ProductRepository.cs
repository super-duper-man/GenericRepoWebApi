using GenericRepoWebApi.Data;
using GenericRepoWebApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace GenericRepoWebApi.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            return await dbSet.Where(p => p.Name.Contains(productName)).ToListAsync();
        }
    }
}
