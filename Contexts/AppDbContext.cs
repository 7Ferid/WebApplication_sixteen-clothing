using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication_sixteen_clothing.Models;

namespace WebApplication_sixteen_clothing.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
