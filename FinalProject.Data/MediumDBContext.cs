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

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.AuthorUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteArticles)
                .HasForeignKey(f => f.UserUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Article)
                .WithMany(a => a.UsersLoveMe)
                .HasForeignKey(f => f.ArticleId);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followed)
                .WithMany(u => u.Followed)
                .HasForeignKey(f => f.FollowedUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowerUsername)
                .HasPrincipalKey(u => u.UserName);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorUsername)
                .HasPrincipalKey(u => u.UserName);
        }
    }
}
