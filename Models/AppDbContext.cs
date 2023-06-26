using LAB2ISTPP.Models;
using Microsoft.EntityFrameworkCore;

public class NewsContext : DbContext
{
    public NewsContext(DbContextOptions<NewsContext> options) : base(options)
    {
    }

    public DbSet<News> News { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Определение связей между моделями
        modelBuilder.Entity<News>()
            .HasOne(n => n.Category)
            .WithMany(c => c.News)
            .HasForeignKey(n => n.CategoryId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.News)
            .WithMany(n => n.Comments)
            .HasForeignKey(c => c.NewsId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);
    }
}
