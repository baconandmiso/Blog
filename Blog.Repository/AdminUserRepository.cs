using Blog.Entity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository;

public class AdminUserRepository : Repository<AdminUser>, IAdminUserRepository
{
    public AdminUserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<AdminUser?> GetByUsernameAsync(string username)
    {
        return await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
    }
}