using Blog.Entity;

namespace Blog.Repository;

public interface IAdminUserRepository : IRepository<AdminUser>
{
    Task<AdminUser?> GetByUsernameAsync(string username);
}