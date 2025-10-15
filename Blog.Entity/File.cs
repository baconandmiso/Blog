namespace Blog.Entity;

/// <summary>
/// 添付ファイルを表すエンティティ。
/// </summary>
public class File : SnowflakeEntity
{
    /// <summary>
    /// 元のファイル名を取得または設定します。
    /// </summary>
    public required string OriginalFileName { get; set; }

    /// <summary>
    /// 保存されたファイル名を取得または設定します。
    /// </summary>
    public required string StoredFileName { get; set; }

    /// <summary>
    /// ファイルの保存パスを取得または設定します。
    /// </summary>
    public required string FilePath { get; set; }

    /// <summary>
    /// ファイルのコンテンツタイプを取得または設定します。
    /// </summary>
    public required string ContentType { get; set; }

    /// <summary>
    /// ファイルのサイズを取得または設定します。
    /// </summary>
    public required long FileSize { get; set; }
}