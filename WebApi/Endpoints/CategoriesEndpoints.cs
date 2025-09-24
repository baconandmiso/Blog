using Blog.Entity;
using Blog.Services;
using Blog.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public static class CategoriesEndpoints
{
    public static void RegisterCategoriesEndpoints(this WebApplication app)
    {
        var categories = app.MapGroup("/api/categories");

        categories.MapPost("/", CreateCategory);
        categories.MapGet("/", GetCategories);
        categories.MapGet("/{id:long}", GetCategory);
        categories.MapPatch("/{id:long}", ModifyCategory);
        categories.MapGet("/{id:long}/articles", GetArticlesByCategoryId);
        categories.MapDelete("/{id:long}", DeleteCategory);
    }

    /// <summary>
    /// カテゴリを作成します
    /// </summary>
    /// <param name="categoryService"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private static async Task<Created<CategoryResponse>> CreateCategory(ICategoryService categoryService, CreateCategoryRequest request)
    {
        var created = await categoryService.CreateAsync(request);

        var response = ToCategoryResponse(created);
        return TypedResults.Created($"/api/categories/{response.Id}", response);
    }

    /// <summary>
    /// 作成したカテゴリをすべて取得します。
    /// </summary>
    /// <param name="categoryService"></param>
    /// <returns></returns>
    private static async Task<Ok<IEnumerable<CategoryResponse>>> GetCategories(ICategoryService categoryService)
    {
        var categories = await categoryService.GetAllAsync();
        var response = categories.Select(ToCategoryResponse);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// カテゴリをIDで取得します
    /// </summary>
    /// <param name="id">カテゴリID</param>
    /// <param name="categoryService"></param>
    /// <returns></returns>
    private static async Task<Results<Ok<CategoryResponse>, NotFound>> GetCategory(long id, ICategoryService categoryService)
    {
        var category = await categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return TypedResults.NotFound();
        }

        var response = ToCategoryResponse(category);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したIDのカテゴリを更新します
    /// </summary>
    /// <param name="id">更新するカテゴリのID</param>
    /// <param name="request"></param>
    /// <param name="categoryService"></param>
    /// <returns></returns>
    private static async Task<Ok<CategoryResponse>> ModifyCategory(long id, UpdateCategoryRequest request, ICategoryService categoryService)
    {
        var updated = await categoryService.UpdateAsync(id, request);
        var response = ToCategoryResponse(updated);
        return TypedResults.Ok(response);
    }

    /// <summary>
    /// 指定したカテゴリIDに紐づく記事をすべて取得します
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="id">カテゴリID</param>
    /// <param name="published"></param>
    /// <returns></returns>
    private static async Task<Ok<IEnumerable<Article>>> GetArticlesByCategoryId(ICategoryService categoryService, long id, [FromQuery] bool published = true)
    {
        if (!published) // 非公開記事も取得, この場合アクセストークンを必要とする。
        {
            throw new NotImplementedException();
        }

        var articles = await categoryService.GetPublishedArticlesByCategory(id);
        return TypedResults.Ok(articles);
    }

    /// <summary>
    /// 指定したIDのカテゴリを削除します
    /// </summary>
    /// <param name="categoryService"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static async Task<NoContent> DeleteCategory(ICategoryService categoryService, long id)
    {
        await categoryService.DeleteAsync(id);
        return TypedResults.NoContent();
    }

    private static CategoryResponse ToCategoryResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
