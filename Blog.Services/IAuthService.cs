using Blog.Shared;

namespace Blog.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}