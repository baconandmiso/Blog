using Blog.Entity;

namespace Blog.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Article>> GetArticlesByCategory(long id);

    Task<IEnumerable<Article>> GetPublishedArticlesByCategory(long id);
}
