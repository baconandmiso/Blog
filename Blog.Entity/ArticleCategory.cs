namespace Blog.Entity;

/// <summary>
/// 記事とカテゴリの多対多の関係を表すエンティティ。
/// </summary>
public class ArticleCategory
{
    /// <summary>
    /// 関連付けられる記事のID(外部キー)を取得または設定します。
    /// </summary>
    public required long ArticleId { get; set; }

    /// <summary>
    /// 関連付けられるカテゴリのID(外部キー)を取得または設定します。
    /// </summary>
    public required long CategoryId { get; set; }

    /// <summary>
    /// 関連付けられる記事エンティティへのナビゲーションプロパティを取得または設定します。
    /// このプロパティはオブジェクトの初期化時のみに設定できます。
    /// </summary>
    public Article Article { get; init; } = null!;

    /// <summary>
    /// 関連付けられるカテゴリエンティティへのナビゲーションプロパティを取得または設定します。
    /// このプロパティはオブジェクトの初期化時のみに設定できます。
    /// </summary>
    public Category Category { get; init; } =  null!;
}
