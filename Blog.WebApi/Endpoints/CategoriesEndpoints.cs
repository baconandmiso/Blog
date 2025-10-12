using Blog.Entity;
using Blog.Services;
using Blog.Shared;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Blog.WebApi.Endpoints;

public static class CategoriesEndpoints
{
    public static void RegisterCategoriesEndpoints(this WebApplication app)
    {
        var categories = app.MapGroup("/api/categories");

        categories.MapPost("/", CreateCategory).RequireAuthorization();
        categories.MapGet("/", GetCategories);
        categories.MapGet("/{id:long}", GetCategory);
        categories.MapPatch("/{id:long}", ModifyCategory).RequireAuthorization();
        categories.MapDelete("/{id:long}", DeleteCategory).RequireAuthorization();
    }

    /// <summary>
    /// カテゴリを作成します
    /// </summary>
    /// <param name="categoryService"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private static async Task<Created<CategoryResponse>> CreateCategory(ICategoryService categoryService, CreateCategoryRequest request)
    {
        Category category = await categoryService.CreateAsync(request);
        CategoryResponse response = category.Adapt<CategoryResponse>();
        return TypedResults.Created($"/api/categories/{response.Id}", response);
    }

    /// <summary>
    /// 作成したカテゴリをすべて取得します。
    /// </summary>
    /// <param name="categoryService"></param>
    /// <returns></returns>
    private static async Task<Ok<IEnumerable<CategoryResponse>>> GetCategories(ICategoryService categoryService)
    {
        IEnumerable<Category> categories = await categoryService.GetAllAsync();
        IEnumerable<CategoryResponse> response = categories.Adapt<IEnumerable<CategoryResponse>>();
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
        Category? category = await categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return TypedResults.NotFound();
        }

        CategoryResponse response = category.Adapt<CategoryResponse>();
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
        Category category = await categoryService.UpdateAsync(id, request);
        CategoryResponse response = category.Adapt<CategoryResponse>();
        return TypedResults.Ok(response);
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
}
