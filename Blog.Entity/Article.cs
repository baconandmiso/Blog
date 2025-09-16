namespace Blog.Entity;

/// <summary>
///　記事を表すエンティティ
/// </summary>
public class Article
{
    /// <summary>
    /// 記事の一意な識別子
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 記事のタイトル
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 記事の内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// サムネイル画像のURL
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// 公開状態を示すフラグ
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// 記事の作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 記事の更新日時
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// 記事を公開した日時
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }

    /// <summary>
    /// カテゴリー
    /// </summary>
    public ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
}
