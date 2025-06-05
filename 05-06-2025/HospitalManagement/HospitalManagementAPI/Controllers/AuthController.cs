using HospitalManagementAPI.DTOs;
using HospitalManagementAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var response = await _authService.Register(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var response = await _authService.Login(request);
            if (!response.Success)
                return BadRequest(response.Message);

            // Returns JWT token on success
            return Ok(new { token = response.Data });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto request)
        {
            var result = await _authService.GoogleLoginAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
