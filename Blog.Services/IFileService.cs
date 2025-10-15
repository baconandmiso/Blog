using Microsoft.AspNetCore.Http;
using File = Blog.Entity.File;

namespace Blog.Services;

public interface IFileService
{
    /// <summary>
    /// ファイルをアップロードします
    /// </summary>
    /// <param name="file"></param>
    /// <param name="subDirectory"></param>
    /// <returns></returns>
    Task<File> UploadAsync(IFormFile file, string subDirectory);

    /// <summary>
    /// 指定されたIDのファイルを取得します
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(Stream Stream, string ContentType, string FileName)?> GetAsync(long id);

    /// <summary>
    /// アップロードしたファイルを削除します
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(long id);
}