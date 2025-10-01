namespace Blog.Shared;

public class LoginResponse
{
    public required string Token { get; set; }

    public bool IsRequiredTotp { get; set; }
}