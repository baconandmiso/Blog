namespace Blog.Entity;

public class UserProfile
{
    public long UserId { get; set; } 
    public string DisplayName { get; set; }
    public string? ProfileText { get; set; }
    public string? IconUrl { get; set; }
    public string? HeaderUrl { get; set; }

    // Userへのナビゲーションプロパティを追加
    public User User { get; set; }
}