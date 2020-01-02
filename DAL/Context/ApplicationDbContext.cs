using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Teg> Tegs { get; set; }
        public DbSet<ArticleTeg> ArticleTegs { get; set; }

        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Blog>()
                .HasOne<User>(b => b.Owner)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article>()
                .HasOne<Blog>(a => a.Blog)
                .WithMany(b => b.Articles)
                .HasForeignKey(a => a.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne<Article>(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne<User>(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleTeg>().HasKey(at => new { at.ArticleId, at.TegId });

            modelBuilder.Entity<ArticleTeg>()
                .HasOne<Article>(at => at.Article)
                .WithMany(a => a.ArticleTegs)
                .HasForeignKey(at => at.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleTeg>()
                .HasOne<Teg>(at => at.Teg)
                .WithMany(t => t.ArticleTegs)
                .HasForeignKey(at => at.TegId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
