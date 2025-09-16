using Microsoft.EntityFrameworkCore;

namespace Blog.Repository;

/// <summary>
/// Repositoryの基本実装
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Entity IDでエンティティを取得します
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns></returns>
    public virtual async Task<T?> GetByIdAsync(long id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Entityを全件取得します
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Entityを追加します
    /// </summary>
    /// <param name="entity">エンティティ</param>
    /// <returns></returns>
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    /// <summary>
    /// Entityを更新します
    /// </summary>
    /// <param name="entity">エンティティ</param>
    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    /// <summary>
    /// Entityを削除します
    /// </summary>
    /// <param name="entity">エンティティ</param>
    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
