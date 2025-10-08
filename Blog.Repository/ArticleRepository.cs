using Blog.Entity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// 公開済みの記事のみ取得
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Article>> GetPublishedArticlesWithDetailsAsync()
    {
        return await _context.Articles
            .Where(a => a.IsPublished)
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .OrderByDescending(a => a.PublishedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 記事をすべて取得します。
    /// </summary>
    /// <returns></returns>
    public override async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _context.Articles
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .ToListAsync();
    }

    /// <summary>
    /// 指定したIDの記事を取得します。
    /// </summary>
    /// <param name="id">記事ID</param>
    /// <returns></returns>
    public override async Task<Article?> GetByIdAsync(long id)
    {
        return await _context.Articles
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
