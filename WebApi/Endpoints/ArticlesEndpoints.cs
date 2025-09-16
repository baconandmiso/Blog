using Blog.Entity;
using Blog.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public static class ArticlesEndpoints
{
    public static void RegisterArticlesEndpoints(this WebApplication app)
    {
        var articles = app.MapGroup("/api/articles");

        articles.MapPost("/", CreateArticle);
        articles.MapGet("/", GetArticles);
        articles.MapGet("/{id:long}", GetArticle);
        articles.MapPatch("/{id:long}", ModifyArticle);
        articles.MapPatch("/{id:long}/thumbnail", ModifyThumbnail);
        articles.MapPost("/{id:long}/publish", PublishArticle);
        articles.MapPost("/{id:long}/unpublish", UnpublishArticle);
        articles.MapDelete("/{id:long}", DeleteArticle);
    }

    /// <summary>
    /// 記事を作成します
    /// </summary>
    /// <param name="request"></param>
    /// <param name="articleService"></param>
    /// <returns></returns>
    private static async Task<Created<ArticleResponse>> CreateArticle(CreateArticleRequest request, IArticleService articleService)
    {
        var created = await articleService.CreateAsync(request.Title, request.Content, request.CategoryIds);

        var article = await articleService.GetByIdAsync(created.Id);
        var response = ToArticleResponse(article!);
        return TypedResults.Created($"/api/articles/{article!.Id}", response);
    }

    /// <summary>
    /// 記事を取得します
    /// </summary>
    /// <param name="articleService"></param>
    /// <returns></returns>
    private static async Task<Ok<IEnumerable<ArticleResponse>>> GetArticles(IArticleService articleService, [FromQuery] bool published = true)
    {
        if (!published) // 非公開記事も取得, この場合アクセストークンを必要とする。
        {
            throw new NotImplementedException();
        }

        var articles = await articleService.GetAllPublishedAsync();

        var response = articles.Select(ToArticleResponse);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したIDの記事を取得します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static async Task<Results<Ok<ArticleResponse>, NotFound>> GetArticle(IArticleService articleService, long id)
    {
        var article = await articleService.GetByIdAsync(id);
        if (article == null)
        {
            return TypedResults.NotFound();
        }

        if (!article.IsPublished) // 非公開記事の場合, アクセストークンを必要とする。
        {
            throw new NotImplementedException();
        }

        var response = ToArticleResponse(article);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したIDの記事を更新します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static async Task<Results<NoContent, NotFound>> ModifyArticle(IArticleService articleService, long id, CreateArticleRequest request)
    {
        var article = await articleService.GetByIdAsync(id);
        if (article == null)
        {
            return TypedResults.NotFound();
        }

        await articleService.UpdateAsync(id, request.Title, request.Content, request.CategoryIds);
        return TypedResults.NoContent();
    }

    /// <summary>
    /// 指定したIDの記事のサムネイルを更新します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static async Task<Results<NoContent, NotFound>> ModifyThumbnail(IArticleService articleService, IWebHostEnvironment env, long id, ThumbnailUpdateRequest request)
    {
        var article = await articleService.GetByIdAsync(id);
        if (article == null)
        {
            return TypedResults.NotFound();
        }

        var webRootPath = env.WebRootPath;
        var success = await articleService.UpdateThumbnailAsync(id, request.Image, webRootPath);

        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    /// <summary>
    /// 指定した記事の公開状態を変更します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static async Task<Results<Ok<ArticleResponse>, NotFound>> PublishArticle(IArticleService articleService, long id)
    {
        var article = await articleService.PublishAsync(id);
        if (article == null)
        {
            return TypedResults.NotFound();
        }

        var response = ToArticleResponse(article);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定した記事の公開状態を変更します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static async Task<Results<Ok<ArticleResponse>, NotFound>> UnpublishArticle(IArticleService articleService, long id)
    {
        var article = await articleService.UnpublishAsync(id);
        if (article == null)
        {
            return TypedResults.NotFound();
        }

        var response = ToArticleResponse(article);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したIDの記事を削除します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static async Task<Results<NoContent, NotFound>> DeleteArticle(IArticleService articleService, long id)
    {
        var article = await articleService.GetByIdAsync(id);
        if (article == null)
        {
            return TypedResults.NotFound();
        }

        await articleService.DeleteAsync(id);
        return TypedResults.NoContent();
    }

    private static ArticleResponse ToArticleResponse(Article article)
    {
        return new ArticleResponse
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Categories = article.ArticleCategories.Select(ac => new CategoryResponse
            {
                Id = ac.Category.Id,
                Name = ac.Category.Name
            }).ToList(),
            ThumbnailUrl = article.ThumbnailUrl,
            IsPublished = article.IsPublished,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt,
            PublishedAt = article.PublishedAt
        };
    }
}

public class ThumbnailUpdateRequest
{
    public string Image { get; set; }
}

public class CreateArticleRequest
{
    public string Title { get; set; }

    public string Content { get; set; }

    public IEnumerable<long> CategoryIds { get; set; }
}

public class CategoryResponse
{
    public long Id { get; set; }

    public string Name { get; set; }
}

public class ArticleResponse
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public ICollection<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    public string? ThumbnailUrl { get; set; }

    public bool IsPublished { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }
}