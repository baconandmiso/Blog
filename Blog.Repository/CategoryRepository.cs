using Blog.Entity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// 特定のカテゴリーに属する記事をすべて取得します。
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <returns></returns>
    public async Task<IEnumerable<Article>> GetArticlesByCategory(long id)
    {
        return await _context.Categories
            .Include(c => c.ArticleCategories)
            .ThenInclude(ac => ac.Article)
            .Where(c => c.Id == id)
            .SelectMany(c => c.ArticleCategories.Select(ac => ac.Article))
            .ToListAsync();
    }

    /// <summary>
    /// 特定のカテゴリーに属する公開済みの記事をすべて取得します。
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <returns></returns>
    public async Task<IEnumerable<Article>> GetPublishedArticlesByCategory(long id)
    {
        return await _context.Categories
            .Include(c => c.ArticleCategories)
            .ThenInclude(ac => ac.Article)
            .Where(c => c.Id == id)
            .SelectMany(c => c.ArticleCategories.Select(ac => ac.Article))
            .Where(a => a.IsPublished)
            .ToListAsync();
    }
}
