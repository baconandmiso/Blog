using Blog.Entity;

namespace Blog.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByNameAsync(string name);
}