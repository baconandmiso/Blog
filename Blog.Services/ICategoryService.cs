using Blog.Entity;
using Blog.Shared;

namespace Blog.Services;

/// <summary>
/// カテゴリーに関するビジネスロジックを提供します。
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// 新しいカテゴリーを作成します。
    /// </summary>
    /// <param name="name">新しいカテゴリーの名前。</param>
    /// <returns>作成された<see cref="Category"/>オブジェクトを含むタスク。</returns>
    Task<Category> CreateAsync(CreateCategoryRequest request);

    /// <summary>
    /// 既存のカテゴリー情報を更新します。
    /// </summary>
    /// <param name="id">更新するカテゴリーのID。</param>
    /// <param name="request"></param>
    /// <returns>
    /// 更新された<see cref="Category"/>オブジェクトを含むタスク。
    /// カテゴリーが見つからない場合は, 結果がnullになります。
    /// </returns>
    Task<Category> UpdateAsync(long id, UpdateCategoryRequest request);

    /// <summary>
    /// 指定されたIDのカテゴリーを削除します。
    /// </summary>
    /// <param name="id">削除するカテゴリーのID。</param>
    /// <returns>操作の完了を表す<see cref="Task"/>。</returns>
    Task DeleteAsync(long id);

    /// <summary>
    /// すべてのカテゴリーを取得します。
    /// </summary>
    /// <returns>すべての<see cref="Category"/>オブジェクトのシーケンスを含むタスク。</returns>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// 指定されたIDのカテゴリーを取得します。
    /// </summary>
    /// <param name="id">取得するカテゴリーのID。</param>
    /// <returns>
    /// 見つかった<see cref="Category"/>オブジェクトを含むタスク。
    /// カテゴリーが見つからない場合は, 結果がnullになります。
    /// </returns>
    Task<Category?> GetByIdAsync(long id);

    /// <summary>
    /// 指定されたカテゴリーIDに関連付けられた記事を取得します。
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <returns></returns>
    Task<IEnumerable<Article>> GetArticlesByCategory(long id);

    /// <summary>
    /// 指定されたカテゴリーIDに関連付けられた公開済み記事を取得します。
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <returns></returns>
    Task<IEnumerable<Article>> GetPublishedArticlesByCategory(long id);
}
