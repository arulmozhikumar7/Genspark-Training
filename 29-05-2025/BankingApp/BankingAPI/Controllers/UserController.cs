using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BankingAPI.DTOs;
using BankingAPI.Services;

namespace BankingAPI.Controllers
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
        }
    }
}
