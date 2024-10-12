using Microsoft.EntityFrameworkCore;
using NAIS_Website.Models;
using System.Security.Cryptography.X509Certificates;

namespace NAIS_Website.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CatalogCategory> CatalogCategory { get; set; }
        public DbSet<Catalog> Catalog { get; set; }
    }
}