using Blog.Entity;
using Blog.Repository;

namespace Blog.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;

    private readonly ApplicationDbContext _context;

    public ArticleService(IArticleRepository articleRepository, ApplicationDbContext context)
    {
        _articleRepository = articleRepository;
        _context = context;
    }

    public async Task<Article> CreateArticleAsync(string title, string content, IEnumerable<ulong> categoryIds)
    {
        var article = new Article
        {
            Id = SnowflakeService.GenerateId(),
            Title = title,
            Content = content,
            IsPublished = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _articleRepository.AddAsync(article);

        await _context.SaveChangesAsync();

        return article;
    }

    public Article UpdateArticleAsync(ulong articleId, string title, string content, IEnumerable<ulong> categoryIds)
    {
        throw new NotImplementedException();
    }

    public void DeleteArticle(ulong articleId)
    {
        throw new NotImplementedException();
    }

    public async Task<Article?> GetArticleAsync(ulong articleId)
    {
        return await _articleRepository.GetByIdAsync(articleId);
    }

    public async Task<IEnumerable<Article>> GetAllArticlesAsync()
    {
        return await _articleRepository.GetAllAsync();
    }

    public async Task<Article?> GetPublishedArticleAsync(ulong articleId)
    {
        var publishedArticles = await _articleRepository.GetPublishedArticlesWithDetailsAsync();
        var punlishedArticle = publishedArticles.FirstOrDefault(a => a.Id == articleId);
        if (punlishedArticle is null)
        {
            return null;
        }

        return punlishedArticle;
    }

    public async Task<IEnumerable<Article>> GetPublishedArticlesAsync()
    {
        return await _articleRepository.GetPublishedArticlesWithDetailsAsync();
    }
}
