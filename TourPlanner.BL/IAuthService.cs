namespace TourPlanner.BL;

using TourPlanner.Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public interface IAuthService
{
    AuthResult Register(string email, string password);
    AuthResult Login(string email, string password);
}
