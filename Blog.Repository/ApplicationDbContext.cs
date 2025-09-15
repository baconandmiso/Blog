using Microsoft.EntityFrameworkCore;
using Blog.Entity;

namespace Blog.Repository;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 記事
    /// </summary>
    public DbSet<Article> Articles { get; set; }

    /// <summary>
    /// カテゴリー
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// 記事 - カテゴリー の関連
    /// </summary>
    public DbSet<ArticleCategory> ArticleCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ArticleCategory>()
            .HasKey(ac => new { ac.ArticleId, ac.CategoryId });
    }
}
