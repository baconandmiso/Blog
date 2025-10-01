using System.Net;
using Blog.Entity;
using Blog.Repository;
using Blog.Shared;
using Org.BouncyCastle.OpenSsl;

namespace Blog.Services;

public class AuthService : IAuthService
{
    private readonly IAdminUserRepository _adminUserRepository;

    private readonly IPasswordHasher _passwordHasher;

    private readonly IJwtProvider _jwtProvider;

    public AuthService(IAdminUserRepository adminUserRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _adminUserRepository = adminUserRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        // 1. ユーザーを検索
        var user = await _adminUserRepository.GetByUsernameAsync(request.Username);
        if (user is null)
        {
            throw new UnauthorizedAccessException("ユーザー名またはパスワードが正しくありません。");
        }

        // 2. パスワードを検証
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("ユーザー名またはパスワードが正しくありません。");
        }

        // 3. TOTPが有効かチェック
        if (string.IsNullOrEmpty(user.TotpSecretKey))
        {
            // TOTPなし -> すぐにトークンを発行
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        return "";
    }

}