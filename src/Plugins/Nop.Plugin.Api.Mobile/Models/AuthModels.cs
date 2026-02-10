namespace Nop.Plugin.Api.Mobile.Models;

public class LoginRequest
{
    public string UsernameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? CustomerId { get; set; }
    public string? Email { get; set; }
    public string? ErrorMessage { get; set; }
}
