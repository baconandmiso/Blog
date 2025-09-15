using Blog.Entity;

namespace Blog.Services;

public interface IArticleService
{
    Task<Article> CreateArticleAsync(string title, string content, IEnumerable<ulong> categoryIds);

    Article UpdateArticleAsync(ulong articleId, string title, string content, IEnumerable<ulong> categoryIds);

    Task<Article?> GetArticleAsync(ulong articleId);

    void DeleteArticle(ulong articleId);

    Task<IEnumerable<Article>> GetAllArticlesAsync();

    Task<IEnumerable<Article>> GetPublishedArticlesAsync();

    Task<Article?> GetPublishedArticleAsync(ulong articleId);
}
