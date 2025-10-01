namespace Blog.Entity;

public class AdminUser
{
    public long Id { get; set; }

    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public string? TotpSecretKey { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}