namespace TourPlanner.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using TourPlanner.BL;

public class AuthRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public ActionResult Register(AuthRequest request)
    {
        var result = _authService.Register(request.Email, request.Password);
        if (!result.Success)
            return Conflict(new { error = result.Error });

        return Ok(new { token = result.Token, email = result.Email });
    }

    [HttpPost("login")]
    public ActionResult Login(AuthRequest request)
    {
        var result = _authService.Login(request.Email, request.Password);
        if (!result.Success)
            return Unauthorized(new { error = result.Error });

        return Ok(new { token = result.Token, email = result.Email });
    }
}
