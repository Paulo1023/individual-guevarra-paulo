using Microsoft.EntityFrameworkCore;
using Nexus.Core;

namespace Nexus.Web.Data
{
    public class ApplicationDbContext : DbContext //allows connection between app and database
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; } //access the Supplier table
    }
}
