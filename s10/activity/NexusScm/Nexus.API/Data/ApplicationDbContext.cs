using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;

namespace Nexus.API.Data
{
    //public class ApplicationDbContext : DbContext //allows connection between app and database
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; } //access the Supplier table
        public DbSet<Product> Products { get; set; } //access the Product table
    }
}
