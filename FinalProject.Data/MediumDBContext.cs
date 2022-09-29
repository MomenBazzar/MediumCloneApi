using FinalProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class MediumDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public MediumDbContext(DbContextOptions<MediumDbContext> options) : base(options)
        {
        }
    }
}
