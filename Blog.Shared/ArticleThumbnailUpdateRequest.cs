using System.ComponentModel.DataAnnotations;

namespace Blog.Shared;

/// <summary>
/// 記事のサムネイルを更新するためのDTO。
/// </summary>
public class ArticleThumbnailUpdateRequest
{
    /// <summary>
    /// 新しいサムネイルのIDを取得または設定します。
    /// </summary>
    public long? ThumbnailId { get; set; }
}