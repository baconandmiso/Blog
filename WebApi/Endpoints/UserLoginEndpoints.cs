using Blog.Services;
using Blog.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Endpoints;

public static class UserLoginEndpoints
{
    public static void RegisterUserLoginEndpoints(this WebApplication app)
    {
        app.MapPost("/api/login", Login);
    }

    private static async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult>> Login(LoginRequest request, IUserService userService)
    {
        var user = await userService.ValidateUserAsync(request);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        // TODO: Generate JWT token
        string token = "";
        return TypedResults.Ok(new LoginResponse { Token = token });
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
}