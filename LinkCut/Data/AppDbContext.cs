using LinkCut.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkCut.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ShortLink> ShortLinks { get; set; }

    }
}
