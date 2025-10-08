namespace Blog.Entity;

public class UserProfile : SnowflakeEntity
{
    public required string DisplayName { get; set; }
    public string? ProfileText { get; set; }
    public string? IconUrl { get; set; }
    public string? HeaderUrl { get; set; }
}