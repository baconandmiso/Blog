using Blog.Entity;

namespace Blog.Services;

public interface IJwtProvider
{
    string GenerateToken(AdminUser user);
}