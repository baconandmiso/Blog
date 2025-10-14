using Blog.Entity;
using Microsoft.AspNetCore.Http;

namespace Blog.Services;

public interface IAttachmentService
{
    /// <summary>
    /// ファイルをアップロードします
    /// </summary>
    /// <param name="file"></param>
    /// <param name="subDirectory"></param>
    /// <returns></returns>
    Task<Attachment> UploadAsync(IFormFile file, string subDirectory);

    /// <summary>
    /// アップロードしたファイルを削除します
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(long id);
}