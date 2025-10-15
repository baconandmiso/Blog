using Blog.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _attachmentRepository;

    private readonly ApplicationDbContext _context;

    private readonly ILogger<FileService> _logger;

    /// <summary>
    /// 保存先のパス
    /// </summary>
    private readonly string _storagePath;

    public FileService(IFileRepository attachmentRepository, ApplicationDbContext context, IConfiguration configuration, ILogger<FileService> logger)
    {
        _attachmentRepository = attachmentRepository;

        _context = context;

        _logger = logger;

        // appsettings.jsonから保存先パスを取得。なければデフォルト値を設定します。
        _storagePath = configuration.GetValue<string>("FileStoragePath") ?? Path.Combine(Directory.GetCurrentDirectory(), "Storage");
    }

    /// <summary>
    /// アップロードされたファイルを保存します。
    /// </summary>
    /// <param name="file"></param>
    /// <param name="subDirectory"></param>
    /// <returns></returns>
    public async Task<Entity.File> UploadAsync(IFormFile file, string subDirectory)
    {
        // 1. ファイルのメタデータを取得
        var originalFileName = file.FileName;
        var contentType = file.ContentType;
        var fileSize = file.Length;

        // 2. サーバー上での一意なファイル名とパスを生成
        var fileExtension = Path.GetExtension(originalFileName);
        var storedFileName = $"{Guid.NewGuid()}{fileExtension}";
        var directoryPath = Path.Combine(_storagePath, subDirectory);
        var filePath = Path.Combine(directoryPath, storedFileName);

        // 3. ディレクトリが存在しなければ作成
        Directory.CreateDirectory(directoryPath);

        // 4. ファイルを物理的に保存
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Entity.File
        {
            OriginalFileName = originalFileName,
            StoredFileName = storedFileName,
            FilePath = filePath,
            ContentType = contentType,
            FileSize = fileSize
        };

        await _attachmentRepository.AddAsync(attachment);
        await _context.SaveChangesAsync();

        return attachment;
    }

    /// <summary>
    /// 指定されたIDのファイルを取得します。
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<(Stream Stream, string ContentType, string FileName)?> GetAsync(long id)
    {
        var entity = await _attachmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException();
        }

        var path = Path.Combine(_storagePath, entity.FilePath);
        if (!File.Exists(path))
        {
            return null;
        }

        var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return (fileStream, entity.ContentType, entity.OriginalFileName);
    }

    /// <summary>
    /// アップロードされたファイルを削除します。
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}