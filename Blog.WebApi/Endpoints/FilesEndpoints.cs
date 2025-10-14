using Blog.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Blog.WebApi.Endpoints;

public static class FileEndpoints
{
    public static void RegisterFileEndpoints(this WebApplication app)
    {
        app.MapPost("/api/files/upload", UploadFile).RequireAuthorization();
    }

    public static async Task<Results<Created, BadRequest<string>>> UploadFile(IFormFile file, string subDirectory, IAttachmentService attachmentService)
    {
        if (file == null || file.Length == 0)
        {
            return TypedResults.BadRequest("No file uploaded.");
        }

        var attachment = await attachmentService.UploadAsync(file, subDirectory);
        return TypedResults.Created();
    }
}

