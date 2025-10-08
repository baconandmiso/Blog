using Blog.Entity;

namespace Blog.Repository;

public interface IArticleRepository : IRepository<Article>
{
    /// <summary>
    /// 公開済みの記事を取得します
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Article>> GetPublishedArticlesWithDetailsAsync();
}
