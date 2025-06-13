using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Enums;
using ExpenseTrackerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
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

            if (await _userService.IsEmailTakenAsync(request.Email))
            {
                return Conflict(ApiResponse<object>.ErrorResponse("Email is already registered.", new
                {
                    Email = new[] { "Email is already taken." }
                }));
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Role = Roles.User,
                IsActive = true,
                IsDeleted = false
            };

            var result = await _userService.RegisterUserAsync(user, request.Password);

            if (!result)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Failed to register user.", new
                {
                    General = new[] { "An internal error occurred." }
                }));
            }

            return Ok(ApiResponse<object>.SuccessResponse((object?)null, "User registered successfully"));
        }

        [HttpPost("register-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserRequest request)
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

            if (await _userService.IsEmailTakenAsync(request.Email))
            {
                return Conflict(ApiResponse<object>.ErrorResponse("Email is already registered.", new
                {
                    Email = new[] { "Email is already taken." }
                }));
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Role = Roles.Admin,
                IsActive = true,
                IsDeleted = false
            };

            var result = await _userService.RegisterUserAsync(user, request.Password);

            if (!result)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Failed to register admin.", new
                {
                    General = new[] { "An internal error occurred." }
                }));
            }

            return Ok(ApiResponse<object>.SuccessResponse((object?)null, "Admin registered successfully"));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("User not found", new
                {
                    Id = new[] { "No user found with the given ID." }
                }));
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, "User fetched successfully"));
        }

        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("User not found", new
                {
                    Email = new[] { "No user found with the given email." }
                }));
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, "User fetched successfully"));
        }
    }
}
