using Microsoft.EntityFrameworkCore;
using TechMart.Domain;

namespace TechMart.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        { }

        public DbSet<Articulos> Articulos { get; set; }
    }

}
