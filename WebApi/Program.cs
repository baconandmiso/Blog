using Blog.Entity;
using Blog.Repository;
using Blog.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}

builder.AddServiceDefaults();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var provider = builder.Configuration.GetValue<string>("DatabaseProvider");

    switch (provider)
    {
        case "MySQL":
            options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection") ?? "");
            break;
        case "PostgreSQL":
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
            break;
        case "SQLite":
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
            break;
        default:
            throw new Exception($"Unsupported database provider: {provider}");
    }
});

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var articles = app.MapGroup("/api/articles");

// MEMO: TypedResultsを使うようにしたほうがいいのかもしれない.

// GET /api/articles
//  Get all articles
articles.MapGet("/", async Task<Ok<IEnumerable<Article>>> (IArticleService articleService) =>
    TypedResults.Ok(await articleService.GetAllArticlesAsync()));

// GET /api/articles/{id}
//  Get article by id
articles.MapGet("/{id}", async Task<Results<Ok<Article>, NotFound>> (ulong id, IArticleService articleService) =>
    await articleService.GetArticleAsync(id)
        is Article article
            ? TypedResults.Ok(article)
            : TypedResults.NotFound());

// POST /api/articles
//  Create a new article
articles.MapPost("/", async (ArticleRequest request, IArticleService articleService) =>
{
    var article = await articleService.CreateArticleAsync(request.title, request.content, new List<ulong> { 1, 2, 3 } );
    return Results.Created($"/api/articles/{article.Id}", article);
});

// PUT /api/articles/{id}
//  Update an article
articles.MapPatch("/{id}", (IArticleService articleService, ulong id, string title, string content) =>
{
    var updatedArticle = articleService.UpdateArticleAsync(id, title, content, new List<ulong> { 1, 2, 3 });
    return Results.Ok(updatedArticle);
});

// DELETE /api/articles/{id}
//  Delete an article
articles.MapDelete("/{id}", (IArticleService articleService, ulong id) =>
{
    articleService.DeleteArticle(id);
    return Results.NoContent();
});

var published = articles.MapGroup("/published");

// GET /api/articles/published
//  Get all published articles
published.MapGet("/", async Task<Ok<IEnumerable<Article>>> (IArticleService articleService) =>
    TypedResults.Ok(await articleService.GetPublishedArticlesAsync()));

// GET /api/articles/published/{id}
//  Get published article by id
published.MapGet("/{id}", async Task<Results<Ok<Article>, NotFound>> (ulong id, IArticleService articleService) =>
    await articleService.GetPublishedArticleAsync(id)
        is Article article
            ? TypedResults.Ok(article)
            : TypedResults.NotFound());

// PUT /api/articles/published/{id}
//  Publish an article
published.MapPut("/{id}", async (IArticleService articleService, ulong id) =>
{
    var articles = await articleService.GetAllArticlesAsync();
    var article = articles.FirstOrDefault(a => a.Id == id);
    if (article is null)
    {
        return Results.NotFound();
    }

    // TODO: 公開状態を更新する。
    //  article.IsPublished = true; or article.IsPublished = false; のようにする。

    // TODO: UpdateArticleAsyncメソッドを使って, 変更を保存する。
    //  ただし, UpdateArticleAsyncメソッドは未実装のため, 実装すること。

    return Results.Ok();
});

app.Run();

public class ArticleRequest
{
    public string title { get; set; } = string.Empty;

    public string content { get; set; } = string.Empty;
}