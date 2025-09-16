using Blog.Entity;
using Blog.Repository;

namespace Blog.Services;

/// <summary>
/// 記事に関するビジネスロジックを提供します。
/// </summary>
public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;

    private readonly ApplicationDbContext _context;

    /// <summary>
    /// <see cref="ArticleService"/>の新しいインスタンスを生成します。
    /// </summary>
    /// <param name="articleRepository">記事データにアクセスするためのリポジトリ</param>
    /// <param name="context">データベースのセッションを表すコンテキスト</param>
    public ArticleService(IArticleRepository articleRepository, ApplicationDbContext context)
    {
        _articleRepository = articleRepository;
        _context = context;
    }

    /// <summary>
    /// 新しい記事を作成します。
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="content">本文</param>
    /// <param name="categoryIds">関連付けるカテゴリーID(s)</param>
    /// <returns>作成された<see cref="Article"/>オブジェクトを含むタスク</returns>
    public async Task<Article> CreateAsync(string title, string content, IEnumerable<long> categoryIds)
    {
        long articleId = SnowflakeService.GenerateId();

        var article = new Article
        {
            Id = articleId,
            Title = title,
            Content = content,
            IsPublished = false,
            ArticleCategories = categoryIds.Select(cid => new ArticleCategory
            {
                CategoryId = cid,
                ArticleId = articleId
            }).ToList(),
            CreatedAt = DateTimeOffset.UtcNow
        };

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
    public async Task<Article?> UpdateAsync(long articleId, string title, string content, IEnumerable<long> categoryIds)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            return null;
        }

        if (title is not null)
        {
            article.Title = title;
        }
        if (content is not null)
        {
            article.Content = content;
        }

        // 3. カテゴリが指定されている場合は、既存の関連付けを一度クリアして再設定する
        if (categoryIds is not null)
        {
            // カテゴリIDの存在チェック (Create時と同様)
            // ... (省略) ...

            // 既存のカテゴリ関連付けを削除
            article.ArticleCategories.Clear();

            // 新しいカテゴリ関連付けを追加
            foreach (var categoryId in categoryIds)
            {
                article.ArticleCategories.Add(new ArticleCategory { CategoryId = categoryId, ArticleId = articleId });
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
    /// <param name="articleId">削除する記事のID。</param>
    /// <returns>操作の完了を表す<see cref="Task"/>。</returns>
    public async Task DeleteAsync(long articleId)
    {
        var article = new Article { Id = articleId };

        _articleRepository.Delete(article);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 指定されたIDの記事を取得します。
    /// </summary>
    /// <param name="articleId">取得する記事のID。</param>
    /// <returns>
    /// 見つかった<see cref="Article"/>オブジェクトを含むタスク。
    /// 記事が見つからない場合は，タスクの結果がnullになります。
    /// </returns>
    public async Task<Article?> GetByIdAsync(long articleId)
    {
        return await _articleRepository.GetByIdAsync(articleId);
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
    /// <exception cref="InvalidOperationException">指定されたIDの記事が見つからない場合にスローされます。</exception>
    public async Task<Article> PublishAsync(long articleId)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            throw new InvalidOperationException("記事が見つかりません。");
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
    /// <exception cref="InvalidOperationException">指定されたIDの記事が見つからない場合にスローされます。</exception>
    public async Task<Article> UnpublishAsync(long articleId)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            throw new InvalidOperationException("記事が見つかりません。");
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
    public async Task<bool> UpdateThumbnailAsync(long articleId, string base64Image, string webRootPath)
    {
        var article = await _articleRepository.GetByIdAsync(articleId);
        if (article is null)
        {
            return false;
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

        return true;
    }
}
