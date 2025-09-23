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
    public DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);

        modelBuilder.Entity<UserProfile>()
            .HasKey(p => p.UserId);

        var adminUserName = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        // 環境変数が設定されていない場合は、シーディングを行わずに終了
        if (string.IsNullOrEmpty(adminUserName) || string.IsNullOrEmpty(adminPassword))
        {
            // ログを出力しておくと、変数が設定されていない場合に気づきやすいです
            Console.WriteLine("初期管理者の環境変数が設定されていません。シーディングをスキップします。");
            return;
        }

        // BCryptを使用してパスワードをハッシュ化
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword);

        // HasDataメソッドを使用して初期ユーザーを登録
        modelBuilder.Entity<User>().HasData(
            new User
            {
                    Id = 1, // 主キーは明示的に指定する必要があります
                    Name = adminUserName,
                    PasswordHash = passwordHash
            });
    }
}
