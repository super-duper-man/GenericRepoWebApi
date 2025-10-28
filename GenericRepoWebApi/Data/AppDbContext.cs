using Microsoft.EntityFrameworkCore;

namespace GenericRepoWebApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
    }
}
