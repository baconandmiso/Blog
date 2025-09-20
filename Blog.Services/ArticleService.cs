using AutoMapper;
using Blog.Entity;
using Blog.Repository;
using Blog.Shared;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services;

/// <summary>
/// 記事に関するビジネスロジックを提供します。
/// </summary>
public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;

    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    /// <summary>
    /// <see cref="ArticleService"/>の新しいインスタンスを生成します。
    /// </summary>
    /// <param name="articleRepository">記事データにアクセスするためのリポジトリ</param>
    /// <param name="context">データベースのセッションを表すコンテキスト</param>
    public ArticleService(IArticleRepository articleRepository, ApplicationDbContext context, IMapper mapper)
    {
        _articleRepository = articleRepository;
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// 新しい記事を作成します。
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="content">本文</param>
    /// <param name="categoryIds">関連付けるカテゴリーID(s)</param>
    /// <returns>作成された<see cref="Article"/>オブジェクトを含むタスク</returns>
    public async Task<Article> CreateAsync(CreateArticleRequest request)
    {
        // 指定されたカテゴリーIDが存在するか確認
        if (request.CategoryIds != null && request.CategoryIds.Any())
        {
            var existingCategoryIds = await _context.Categories
                .Where(c => request.CategoryIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            var notFounds = request.CategoryIds.Except(existingCategoryIds).ToList();
            if (notFounds.Count != 0)
            {
                throw new EntityNotFoundException($"Categories not found: {string.Join(", ", notFounds)}");
            }
        }

        // AutoMapperを使用して, DTOからエンティティへのマッピングを行う。
        var article = _mapper.Map<Article>(request);

        // AutoMapperでマッピングしきれない値をここで設定。
        article.Id = SnowflakeService.GenerateId();
        article.IsPublished = false;
        article.CreatedAt = DateTimeOffset.UtcNow;

        foreach (long categoryId in request.CategoryIds!)
        {
            article.ArticleCategories.Add(new ArticleCategory
            {
                CategoryId = categoryId,
                ArticleId = article.Id
            });
        }

        await _articleRepository.AddAsync(article);
        await _context.SaveChangesAsync();

        return article;
    }

    /// <summary>
    /// 既存の記事を更新します。
    /// </summary>
    /// <param name="articleId">更新する記事のID。</param>
    /// <param name="title">新しいタイトル。</param>
    /// <param name="content">新しい本文。</param>
    /// <param name="categoryIds">新しいカテゴリーIDのシーケンス。</param>
    /// <returns>
    /// 更新された<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからない場合は，タスクの結果がnullになります。
    /// </returns>
    public async Task<Article> UpdateAsync(long articleId, UpdateArticleRequest request)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            throw new EntityNotFoundException();
        }

        _mapper.Map(request, article);

        if (request.CategoryIds is not null)
        {
            // 既存のカテゴリ関連付けをクリア
            article.ArticleCategories.Clear();

            // 新しいカテゴリ関連付けを追加
            foreach (var categoryId in request.CategoryIds)
            {
                article.ArticleCategories.Add(new ArticleCategory
                {
                    CategoryId = categoryId,
                    ArticleId = articleId
                });
            }
        }

        article.UpdatedAt = DateTimeOffset.UtcNow;

        _articleRepository.Update(article);
        await _context.SaveChangesAsync();

        return article;
    }

    /// <summary>
    /// 指定されたIDの記事を削除します。
    /// </summary>
    /// <param name="id">削除する記事のID。</param>
    /// <returns>操作の完了を表す<see cref="Task"/>。</returns>
    public async Task DeleteAsync(long id)
    {
        var article = await _articleRepository.GetByIdAsync(id);
        if (article is null)
        {
            throw new EntityNotFoundException();
        }

        _articleRepository.Delete(article);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 指定されたIDの記事を取得します。
    /// </summary>
    /// <param name="id">取得する記事のID。</param>
    /// <returns>
    /// 見つかった<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからない場合は，タスクの結果がnullになります。
    /// </returns>
    public async Task<Article?> GetByIdAsync(long id)
    {
        return await _articleRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// すべての記事を取得します。
    /// </summary>
    /// <returns>すべての<see cref="Article"/>オブジェクトのシーケンスを含むタスク。</returns>
    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _articleRepository.GetAllAsync();
    }

    /// <summary>
    /// 指定されたIDの公開済み記事を取得します。
    /// </summary>
    /// <param name="articleId">取得する記事のID。</param>
    /// <returns>
    /// 見つかった公開済みの<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからないか，非公開の場合は，タスクの結果がnullになります。
    /// </returns>
    public async Task<Article?> GetPublishedByIdAsync(long articleId)
    {
        var publishedArticles = await _articleRepository.GetPublishedArticlesWithDetailsAsync();
        var punlishedArticle = publishedArticles.FirstOrDefault(a => a.Id == articleId);
        if (punlishedArticle is null)
        {
            return null;
        }

        return punlishedArticle;
    }

    /// <summary>
    /// すべての公開済み記事を取得します。
    /// </summary>
    /// <returns>すべての公開済み<see cref="Article"/>オブジェクトのシーケンスを含むタスク。</returns>
    public async Task<IEnumerable<Article>> GetAllPublishedAsync()
    {
        return await _articleRepository.GetPublishedArticlesWithDetailsAsync();
    }

    /// <summary>
    /// 指定されたIDの記事を公開状態に設定します。
    /// </summary>
    /// <param name="articleId">公開する記事のID。</param>
    /// <returns>公開状態に更新された<see cref="Article"/>オブジェクトを含むタスク。</returns>
    /// <exception cref="EntryNotFoundException">指定されたIDの記事が見つからない場合にスローされます。</exception>
    public async Task<Article> PublishAsync(long articleId)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            throw new EntityNotFoundException();
        }

        article.IsPublished = true;
        article.PublishedAt = DateTimeOffset.UtcNow;

        _articleRepository.Update(article);
        await _context.SaveChangesAsync();

        return article;
    }

    /// <summary>
    /// 指定されたIDの記事を非公開状態に設定します。
    /// </summary>
    /// <param name="articleId">非公開にする記事のID。</param>
    /// <returns>非公開状態に更新された<see cref="Article"/>オブジェクトを含むタスク。</returns>
    /// <exception cref="EntityNotFoundException">指定されたIDの記事が見つからない場合にスローされます。</exception>
    public async Task<Article> UnpublishAsync(long articleId)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            throw new EntityNotFoundException();
        }

        article.IsPublished = false;
        article.PublishedAt = null;

        _articleRepository.Update(article);
        await _context.SaveChangesAsync();

        return article;
    }

    /// <summary>
    /// 記事のサムネイル画像を更新します。
    /// </summary>
    /// <param name="articleId">サムネイルを更新する記事のID。</param>
    /// <param name="base64Image">Base64エンコードされた画像データ。</param>
    /// <param name="webRootPath">ファイルを保存するWebルートの物理パス。</param>
    /// <returns>
    /// 更新が成功した場合はtrue，記事が見つからなかった場合はfalseを返すタスク。
    /// </returns>
    public async Task UpdateThumbnailAsync(long articleId, string base64Image, string webRootPath)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            throw new EntityNotFoundException();
        }

        var parts = base64Image.Split(',');
        var imagePart = parts.Length > 1 ? parts[1] : parts[0];

        byte[] imageBytes = Convert.FromBase64String(imagePart);

        var fileName = $"{Guid.NewGuid()}.png";
        var savePath = Path.Combine(webRootPath, "thumbnails", $"{articleId}");
        var filePath = Path.Combine(savePath, fileName);

        Directory.CreateDirectory(savePath);

        await File.WriteAllBytesAsync(filePath, imageBytes);

        var fileUrl = $"/thumbnails/{articleId}/{fileName}";
        article.ThumbnailUrl = fileUrl;

        _articleRepository.Update(article);
        await _context.SaveChangesAsync();
    }
}
