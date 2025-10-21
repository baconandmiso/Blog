using Blog.Entity;
using Blog.Shared;

namespace Blog.Services;

/// <summary>
/// 記事に関するビジネスロジックを提供します。
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// 新しい記事を作成します。
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="content">本文</param>
    /// <param name="categoryIds">関連付けるカテゴリーID(s)</param>
    /// <returns>作成された<see cref="Article"/>オブジェクトを含むタスク</returns>
    Task<Article> CreateAsync(CreateArticleRequest request);

    /// <summary>
    /// 既存の記事を更新します。
    /// </summary>
    /// <param name="articleId">更新する記事のID。</param>
    /// <param name="title">新しいタイトル。</param>
    /// <param name="content">新しい本文。</param>
    /// <param name="categoryIds">新しいカテゴリーIDのシーケンス。</param>
    /// <returns>
    /// 更新された<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからない場合は，タスクの結果がnullになります。
    /// </returns>
    Task<Article> UpdateAsync(long articleId, UpdateArticleRequest request);

    /// <summary>
    /// 指定されたIDの記事を削除します。
    /// </summary>
    /// <param name="id">削除する記事のID。</param>
    /// <returns>操作の完了を表す<see cref="Task"/>。</returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// すべての記事を取得します。
    /// </summary>
    /// <returns>すべての<see cref="Article"/>オブジェクトのシーケンスを含むタスク。</returns>
    Task<IEnumerable<Article>> GetAllAsync();

    /// <summary>
    /// 指定されたIDの記事を取得します。
    /// </summary>
    /// <param name="id">取得する記事のID。</param>
    /// <returns>
    /// 見つかった<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからない場合は，タスクの結果がnullになります。
    /// </returns>
    Task<Article?> GetByIdAsync(long id);

    /// <summary>
    /// すべての公開済み記事を取得します。
    /// </summary>
    /// <returns>すべての公開済み<see cref="Article"/>オブジェクトのシーケンスを含むタスク。</returns>
    Task<IEnumerable<Article>> GetAllPublishedAsync();

    /// <summary>
    /// 指定されたIDの公開済み記事を取得します。
    /// </summary>
    /// <param name="articleId">取得する記事のID。</param>
    /// <returns>
    /// 見つかった公開済みの<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからないか，非公開の場合は，タスクの結果がnullになります。
    /// </returns>
    Task<Article?> GetPublishedByIdAsync(long articleId);

    /// <summary>
    /// 指定されたIDの記事を公開状態に設定します。
    /// </summary>
    /// <param name="articleId">公開する記事のID。</param>
    /// <returns>公開状態に更新された<see cref="Article"/>オブジェクトを含むタスク。</returns>
    Task<Article> PublishAsync(long articleId);

    /// <summary>
    /// 指定されたIDの記事を非公開状態に設定します。
    /// </summary>
    /// <param name="articleId">非公開にする記事のID。</param>
    /// <returns>非公開状態に更新された<see cref="Article"/>オブジェクトを含むタスク。</returns>
    Task<Article> UnpublishAsync(long articleId);

    /// <summary>
    /// 記事のサムネイル画像を更新します。
    /// </summary>
    /// <param name="articleId">サムネイルを更新する記事のID。</param>
    /// <param name="fileId">新しいサムネイル画像のファイルID。</param>
    /// <returns>
    /// 更新が成功した場合はtrue，記事が見つからなかった場合はfalseを返すタスク。
    /// </returns>
    Task UpdateThumbnailAsync(long articleId, long? fileId);
}
