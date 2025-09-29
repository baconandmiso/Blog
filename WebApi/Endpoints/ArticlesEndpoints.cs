using Blog.Entity;
using Blog.Services;
using Blog.Shared;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

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
    private static async Task<Results<Created<ArticleResponse>, ValidationProblem>> CreateArticle(CreateArticleRequest request, IArticleService articleService)
    {
        if (!MiniValidator.TryValidate(request, out var errors))
        {
            return TypedResults.ValidationProblem(errors);
        }

        Article article = await articleService.CreateAsync(request);
        ArticleResponse response = article.Adapt<ArticleResponse>();

        return TypedResults.Created($"/api/articles/{response.Id}", response);
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
    private static async Task<Ok<ArticleResponse>> ModifyArticle(IArticleService articleService, long id, UpdateArticleRequest request)
    {
        var article =  await articleService.UpdateAsync(id, request);
        var response = ToArticleResponse(article);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したIDの記事のサムネイルを更新します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    private static async Task<NoContent> ModifyThumbnail(IArticleService articleService, IWebHostEnvironment env, long id, ThumbnailUpdateRequest request)
    {
        var webRootPath = env.WebRootPath;
        await articleService.UpdateThumbnailAsync(id, request.Image!, webRootPath);
        return TypedResults.NoContent();
    }

    /// <summary>
    /// 指定した記事の公開状態を変更します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static async Task<Ok<ArticleResponse>> PublishArticle(IArticleService articleService, long id)
    {
        var article = await articleService.PublishAsync(id);
        var response = ToArticleResponse(article);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定した記事の公開状態を変更します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static async Task<Ok<ArticleResponse>> UnpublishArticle(IArticleService articleService, long id)
    {
        var article = await articleService.UnpublishAsync(id);
        var response = ToArticleResponse(article);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したIDの記事を削除します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static async Task<NoContent> DeleteArticle(IArticleService articleService, long id)
    {
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
    public string? Image { get; set; }
}
