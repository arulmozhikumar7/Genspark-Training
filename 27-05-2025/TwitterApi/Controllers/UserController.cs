using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAPI.Models;
using TwitterAPI.Services;
using TwitterAPI.Dtos;

namespace TwitterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Register new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Bio = dto.Bio
            };

            var success = await _userService.AddUserAsync(user, dto.Password);
            if (!success)
                return Conflict("Username or email already exists.");

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // Get user by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = new UserDto(user);
            return Ok(result);
        }

        // Get all users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync(); 
            var result = new List<UserDto>();
            foreach (var user in users)
            {
                result.Add(new UserDto(user));
            }
            return Ok(result);
        }

        // Update user
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.Bio = dto.Bio ?? user.Bio;
            user.Email = dto.Email ?? user.Email;

            await _userService.UpdateAsync(user);
            return NoContent();
        }

        // Delete user
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
