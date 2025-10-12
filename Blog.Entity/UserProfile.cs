namespace Blog.Entity;

/// <summary>
/// ユーザープロファイルを表すエンティティ。
/// </summary>
public class UserProfile : SnowflakeEntity
{
    /// <summary>
    /// 表示名を取得または設定します。
    /// </summary>
    public required string DisplayName { get; set; }

    /// <summary>
    /// 略歴を取得または設定します。
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// アイコンのURLを取得または設定します。
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    /// ヘッダー画像のURLを取得または設定します。
    /// </summary>
    public string? HeaderUrl { get; set; }
}