namespace Blog.Entity;

public class User
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public required string PasswordHash { get; set; }

    public UserProfile? Profile { get; set; }
}