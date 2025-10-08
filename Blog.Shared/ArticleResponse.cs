namespace Blog.Shared;

public class ArticleResponse
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public ICollection<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    public string? ThumbnailUrl { get; set; }

    public bool IsPublished { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }
}