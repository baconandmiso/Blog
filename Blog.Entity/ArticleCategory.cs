namespace Blog.Entity;

/// <summary>
/// 記事とカテゴリーの関連を表すエンティティ
/// </summary>
public class ArticleCategory
{
    /// <summary>
    /// 記事ID
    /// </summary>
    public required ulong ArticleId { get; set; }

    /// <summary>
    /// カテゴリID
    /// </summary>
    public required ulong CategoryId { get; set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();

    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
