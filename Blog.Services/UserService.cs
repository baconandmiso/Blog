using Blog.Entity;
using Blog.Repository;
using Blog.Shared;

namespace Blog.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> ValidateUserAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByNameAsync(request.UserName);
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
}