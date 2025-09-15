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
            .Include(a => a.articles)
            .OrderByDescending(a => a.PublishedAt)
            .ToListAsync();
    }
}
