namespace Blog.Entity;

/// <summary>
/// 管理者ユーザーを表すエンティティ。
/// </summary>
public class AdminUser : SnowflakeEntity
{
    /// <summary>
    /// ユーザー名を取得または設定します。
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// パスワードのハッシュを取得または設定します。
    /// </summary>
    public required string PasswordHash { get; set; }
}