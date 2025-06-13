using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value?.Errors?.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            var response = await _authService.AuthenticateAsync(request);

            if (response == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid email or password", new
                {
                    Credentials = new[] { "Invalid email or password." }
                }));
            }

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(response, "Login successful"));
        }

        // POST: api/auth/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value?.Errors?.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            var response = await _authService.RefreshAccessTokenAsync(request.Token, request.RefreshToken);

            if (response == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid or expired refresh token", new
                {
                    RefreshToken = new[] { "Invalid or expired refresh token." }
                }));
            }

            return Ok(ApiResponse<AuthResponse>.SuccessResponse(response, "Token refreshed successfully"));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            var principal = _authService.ValidateToken(request.Token);
            if (principal == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid token", new
                {
                    Token = new[] { "Invalid or expired access token." }
                }));
            }

            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _authService.LogoutAsync(userId, request.RefreshToken);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Logout successful"));
        }


    }
}
