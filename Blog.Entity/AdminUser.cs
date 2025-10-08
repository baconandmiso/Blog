namespace Blog.Entity;

public class AdminUser : SnowflakeEntity
{
    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public string? TotpSecretKey { get; set; }
}