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
    /// 記事のコレクションへのアクセスを提供します。データベースのArticlesテーブルに対応します。
    /// </summary>
    public DbSet<Article> Articles { get; set; }

    /// <summary>
    /// カテゴリのコレクションへのアクセスを提供します。データベースのCategoriesテーブルに対応します。
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// 記事とカテゴリの関連付けコレクションへのアクセスを提供します。
    /// データベースのArticleCategoriesテーブルに対応します。
    /// </summary>
    public DbSet<ArticleCategory> ArticleCategories { get; set; }

    /// <summary>
    /// ユーザーのコレクションへのアクセスを提供します。データベースのUsersテーブルに対応します。
    /// </summary>
    public DbSet<AdminUser> AdminUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ArticleCategory エンティティの複合主キーの設定
        modelBuilder.Entity<ArticleCategory>()
            .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

        // Article と Category の多対多の関係の設定
        modelBuilder.Entity<ArticleCategory>()
            .HasOne(ac => ac.Article)
            .WithMany(a => a.ArticleCategories)
            .HasForeignKey(ac => ac.ArticleId);

        modelBuilder.Entity<ArticleCategory>()
            .HasOne(ac => ac.Category)
            .WithMany(c => c.ArticleCategories)
            .HasForeignKey(ac => ac.CategoryId);
    }
}
