namespace Blog.Shared;

/// <summary>
/// 記事のレスポンスデータを表します。
/// クライアントに返される記事の詳細情報を含みます。
/// </summary>
public class ArticleResponse
{
    /// <summary>
    /// このオブジェクトの一意の識別子を取得します。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 記事のタイトルを取得または設定します。
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 記事の内容を取得または設定します。
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// 記事に関連付けられたカテゴリのコレクションを取得または設定します。
    /// デフォルトでは空のリストで初期化されます。
    /// </summary>
    public ICollection<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    /// <summary>
    /// 記事のサムネイル画像のURLを取得または設定します。
    /// 未設定の場合は<c>null</c>になります。
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// この記事が公開されているかどうかを示す値を取得または設定します。
    /// <c>true</c>であれば公開されており、<c>false</c>であれば非公開です。
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// エンティティが最初に作成された時刻を表す <see cref="DateTimeOffset"/>。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// この記事が最後に更新された時刻を表す <see cref="DateTimeOffset"/>。
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// この記事が公開された時刻を表す <see cref="DateTimeOffset"/>。
    /// </summary>
    public DateTimeOffset? PublishedAt { get; set; }
}