namespace Blog.Entity;

/// <summary>
///　記事を表すエンティティ。
/// </summary>
public class Article
{
    /// <summary>
    /// 記事の一意なIDを取得または設定します。
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 記事のタイトルを取得または設定します。
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 記事の本文を取得または設定します。
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// 記事のサムネイル画像のURLを取得または設定します。
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// 記事が公開されているかどうかを示す値を取得または設定します。
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// 記事の作成日時を取得または設定します。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 記事の最終更新日を取得または設定します。
    /// 更新されていない場合はnullになります。
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// 記事の公開日時を取得または設定します。
    /// 公開されていない場合はnullになります。
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }

    /// <summary>
    /// この記事に関連付けられているカテゴリのコレクションを取得します。
    /// </summary>
    public ICollection<ArticleCategory> ArticleCategories { get; private set; } = new List<ArticleCategory>();
}
