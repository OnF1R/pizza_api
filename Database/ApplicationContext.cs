using Microsoft.EntityFrameworkCore;
using pizza_api.Models;

namespace pizza_api.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=postgres;Password=password;Database=pizza_api;");
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
