using System.ComponentModel.DataAnnotations;

namespace Blog.Shared;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}