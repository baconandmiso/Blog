using Blog.Shared;

namespace Blog.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest request);
}