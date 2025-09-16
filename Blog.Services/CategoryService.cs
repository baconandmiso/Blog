using Blog.Entity;
using Blog.Repository;

namespace Blog.Services;

/// <summary>
/// カテゴリーに関するビジネスロジックを提供します。
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly ApplicationDbContext _context;

    /// <summary>
    /// <see cref="CategoryService"/>の新しいインスタンスを生成します。
    /// </summary>
    /// <param name="categoryRepository">カテゴリーデータにアクセスするためのリポジトリ</param>
    /// <param name="context">データベースのセッションを表すコンテキスト</param>
    public CategoryService(ICategoryRepository categoryRepository, ApplicationDbContext context)
    {
        _categoryRepository = categoryRepository;
        _context = context;
    }

    /// <summary>
    /// 新しいカテゴリーを作成します。
    /// </summary>
    /// <param name="name">新しいカテゴリーの名前。</param>
    /// <returns>作成された<see cref="Category"/>オブジェクトを含むタスク。</returns>
    public async Task<Category> CreateAsync(string name)
    {
        long categoryId = SnowflakeService.GenerateId();

        var category = new Category
        {
            Id = categoryId,
            Name = name
        };

        await _categoryRepository.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }

    /// <summary>
    /// 既存のカテゴリー情報を更新します。
    /// </summary>
    /// <param name="id">更新するカテゴリーのID。</param>
    /// <param name="name">新しいカテゴリーの名前。</param>
    /// <returns>
    /// 更新された<see cref="Category"/>オブジェクトを含むタスク。
    /// カテゴリーが見つからない場合は, 結果がnullになります。
    /// </returns>
    public async Task<Category?> UpdateAsync(long id, string name)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return null;
        }

        if (name is not null)
        {
            category.Name = name;
        }

        _categoryRepository.Update(category);
        await _context.SaveChangesAsync();

        return category;
    }

    /// <summary>
    /// 指定されたIDのカテゴリーを削除します。
    /// </summary>
    /// <param name="id">削除するカテゴリーのID。</param>
    /// <returns>操作の完了を表す<see cref="Task"/>。</returns>
    public async Task DeleteAsync(long id)
    {
        var category = new Category { Id = id };

        _categoryRepository.Delete(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// すべてのカテゴリーを取得します。
    /// </summary>
    /// <returns>すべての<see cref="Category"/>オブジェクトのシーケンスを含むタスク。</returns>
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    /// <summary>
    /// 指定されたIDのカテゴリーを取得します。
    /// </summary>
    /// <param name="id">取得するカテゴリーのID。</param>
    /// <returns>
    /// 見つかった<see cref="Category"/>オブジェクトを含むタスク。
    /// カテゴリーが見つからない場合は, 結果がnullになります。
    /// </returns>
    public async Task<Category?> GetByIdAsync(long id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }
}
