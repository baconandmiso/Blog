namespace Blog.Entity;

/// <summary>
/// 記事とカテゴリーの関連を表すエンティティ
/// </summary>
public class ArticleCategory
{
    /// <summary>
    /// 記事ID
    /// </summary>
    public required long ArticleId { get; set; }

    /// <summary>
    /// カテゴリID
    /// </summary>
    public required long CategoryId { get; set; }

    /// <summary>
    /// 記事
    /// </summary>
    public Article Article { get; set; }

    /// <summary>
    /// カテゴリ
    /// </summary>
    public Category Category { get; set; }
}
