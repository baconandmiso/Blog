namespace Blog.Shared;

public class CreateArticleRequest
{
    public string Title { get; set; }

    public string Content { get; set; }

    public IEnumerable<long> CategoryIds { get; set; }
}
