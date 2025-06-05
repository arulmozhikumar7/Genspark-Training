using Microsoft.AspNetCore.Mvc;
using NotifyAPI.Models;
using NotifyAPI.Interfaces;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.AuthenticateAsync(request.Username, request.Password);
        if (token == null)
            return Unauthorized(new { message = "Invalid username or password" });

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
       
        var user = new User
        {
            Username = request.Username,
            Role = request.Role 
        };

        var createdUser = await _authService.RegisterAsync(user, request.Password);
        return CreatedAtAction(nameof(Register), new { id = createdUser.Id }, new { createdUser.Username, createdUser.Role });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RegisterRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User";
}
