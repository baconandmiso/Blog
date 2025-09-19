namespace Blog.Shared;

public class UpdateArticleRequest
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public IEnumerable<long>? CategoryIds { get; set; }
}
