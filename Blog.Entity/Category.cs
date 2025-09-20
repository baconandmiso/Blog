namespace Blog.Entity;

/// <summary>
/// 記事のカテゴリを表すエンティティ。
/// </summary>
public class Category
{
    /// <summary>
    /// カテゴリの一意なIDを取得または設定します。
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// カテゴリの名前を取得または設定します。
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// カテゴリに関連付けられている記事のコレクションを取得します。
    /// </summary>
    public ICollection<ArticleCategory> ArticleCategories { get; private　set; } = new List<ArticleCategory>();
}
