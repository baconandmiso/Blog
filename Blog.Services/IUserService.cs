using Blog.Entity;
using Blog.Shared;

namespace Blog.Services;

public interface IUserService
{
    Task<User?> ValidateUserAsync(LoginRequest request);
}