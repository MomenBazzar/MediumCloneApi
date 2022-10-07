using FinalProject.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class MediumDbContext : IdentityDbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public MediumDbContext(DbContextOptions<MediumDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Articles)
                .WithOne(u => u.Author)
                .HasForeignKey(a => a.AuthorUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteArticles)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<Article>()
                .HasMany(u => u.UsersLoveMe)
                .WithOne(f => f.Article)
                .HasForeignKey(f => f.ArticleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followed)
                .WithOne(f => f.Followed)
                .HasForeignKey(f => f.FollowedUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithOne(f => f.Follower)
                .HasForeignKey(f => f.FollowerUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Article)
                .HasForeignKey(c => c.ArticleId);
            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorUsername)
                .HasPrincipalKey(u => u.UserName);
        }
    }
}
