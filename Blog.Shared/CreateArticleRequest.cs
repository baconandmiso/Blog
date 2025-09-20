using System.ComponentModel.DataAnnotations;

namespace Blog.Shared;

/// <summary>
/// 新しい記事を作成するためのデータを表すDTO。
/// </summary>
public class CreateArticleRequest
{
    /// <summary>
    /// 記事のタイトルを取得または設定します。
    /// このフィールドは必須です。
    /// </summary>
    [Required(ErrorMessage = "タイトルは必須です。")]
    [StringLength(100, ErrorMessage = "タイトルは100文字以内で入力してください。")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 記事の本文を取得または設定します。
    /// このフィールドは必須です。
    /// </summary>
    [Required(ErrorMessage = "本文は必須です。")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 記事に関連付けるカテゴリIDのコレクションを取得または設定します。
    /// このフィールドはオプションです。指定しない場合は空のリストとして扱われます、
    /// </summary>
    public IEnumerable<long> CategoryIds { get; set; } = new List<long>();
}
