using System.Text.RegularExpressions;

namespace TourPlanner.BL;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TourPlanner.DAL;
using TourPlanner.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public AuthResult Register(string email, string password)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return new AuthResult { Success = false, Error = "The email format is not correct" };

        if (password.Length < 6)
            return new AuthResult { Success = false, Error = "The password length must be greater than 6 characters" };

        if (_userRepository.GetByEmail(email) is not null)
            return new AuthResult { Success = false, Error = "Email already registered" };

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _userRepository.Add(user);
        var token = GenerateToken(user);
        return new AuthResult { Success = true, Token = token, Email = user.Email };
    }

    public AuthResult Login(string email, string password)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return new AuthResult { Success = false, Error = "The email format is not correct" };

        if (password.Length < 6)
            return new AuthResult { Success = false, Error = "The password length must be greater than 6 characters" };

        var user = _userRepository.GetByEmail(email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new AuthResult { Success = false, Error = "Invalid email or password" };

        var token = GenerateToken(user);
        return new AuthResult { Success = true, Token = token, Email = user.Email };
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
