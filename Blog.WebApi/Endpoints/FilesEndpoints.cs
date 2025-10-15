using Blog.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi.Endpoints;

public static class FileEndpoints
{
    public static void RegisterFileEndpoints(this WebApplication app)
    {
        app.MapPost("/api/files/upload", UploadFile).RequireAuthorization().WithMetadata(new RequestSizeLimitAttribute(104857600)).DisableAntiforgery();
        app.MapGet("/api/files/{id:long}", GetFile).WithName("GetFile").RequireAuthorization();
    }

    public static async Task<Results<Created<Entity.File>, BadRequest<string>, ProblemHttpResult>> UploadFile(IFormFile file, [FromForm] string subDirectory, IFileService fileService, LinkGenerator linker)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return TypedResults.BadRequest("No file uploaded.");
            }

            var attachment = await fileService.UploadAsync(file, subDirectory);
            var url = linker.GetPathByName("GetFile", new { id = attachment.Id });

            return TypedResults.Created(url, attachment);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task<Results<FileStreamHttpResult, NotFound>> GetFile(long id, IFileService fileService)
    {
        var fileData = await fileService.GetAsync(id);

        if (fileData is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.File(fileData.Value.Stream, fileData.Value.ContentType, fileData.Value.FileName);
    }
}

