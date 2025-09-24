using System.ComponentModel.DataAnnotations;

namespace Blog.Shared;

/// <summary>
/// 既存の記事を更新するためのデータを表すDTO。
/// すべてのプロパティはオプションです。指定されたプロパティのみが更新されます。
/// </summary>
public class UpdateArticleRequest
{
    /// <summary>
    /// 記事のタイトルを取得または設定します。
    /// nullの場合,タイトルは更新されません。
    /// </summary>
    [StringLength(100, ErrorMessage = "タイトルは100文字以内で入力してください。")]
    public string? Title { get; set; }

    /// <summary>
    /// 記事の新しい本文を取得または設定します。
    /// nullの場合,本文は更新されません。
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 記事に関連付ける新しいカテゴリIDのコレクションを取得または設定します。
    /// nullの場合,カテゴリは更新されません。
    /// 空のリストを指定した場合,すべてのカテゴリが記事から削除されます。
    /// </summary>
    public IEnumerable<long>? CategoryIds { get; set; }

    /// <summary>
    /// 記事が公開されているかどうかを示す値を取得または設定します。
    /// </summary>
    public bool? IsPublished { get; set; }
}
