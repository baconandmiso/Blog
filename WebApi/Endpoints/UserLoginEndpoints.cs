using Blog.Services;
using Blog.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Endpoints;

public static class UserLoginEndpoints
{
    public static void RegisterUserLoginEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", Login);
    }

    private static async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult>> Login(LoginRequest request, IAuthService authService)
    {
        string token = await authService.LoginAsync(request);
        return TypedResults.Ok(new LoginResponse { Token = token });
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
}