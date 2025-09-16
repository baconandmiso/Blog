namespace Blog.Entity;

/// <summary>
/// 記事のカテゴリーを表すエンティティ
/// </summary>
public class Category
{
    /// <summary>
    /// カテゴリーの一意な識別子
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// カテゴリー名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// カテゴリ - 記事の関連
    /// </summary>
    public ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
}
