namespace Blog.Repository;

/// <summary>
/// Repositoryの基本インターフェース
/// </summary>
/// <typeparam name="T">エンティティクラス</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// IDでエンティティを取得します
    /// </summary>
    /// <param name="id">エンティティID</param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(ulong id);

    /// <summary>
    /// エンティティを全件取得します
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// エンティティを追加します
    /// </summary>
    /// <param name="entity">エンティティ</param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    /// エンティティを更新します
    /// </summary>
    /// <param name="entity">エンティティ</param>
    void Update(T entity);

    /// <summary>
    /// エンティティを削除します
    /// </summary>
    /// <param name="entity">エンティティ</param>
    void Delete(T entity);
}
