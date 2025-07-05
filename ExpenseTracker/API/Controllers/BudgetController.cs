using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "User")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _service;

        public BudgetController(IBudgetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] BudgetQueryParameters filters)
        {
            var userId = GetUserId();
            var result = await _service.GetFilteredAsync(userId, filters);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Filtered budgets fetched successfully"));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = GetUserId();
            try
            {
                var budget = await _service.GetByIdAsync(id, userId);
                return Ok(ApiResponse<object>.SuccessResponse(budget, "Budget fetched successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Budget not found", new { id = new[] { "Invalid budget ID." } }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BudgetCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );
                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            var userId = GetUserId();
            await _service.CreateAsync(userId, dto);
            return Ok(ApiResponse<object>.SuccessResponse(null!, "Budget created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BudgetUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );
                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            var userId = GetUserId();
            try
            {
                await _service.UpdateAsync(userId, id, dto);
                return Ok(ApiResponse<object>.SuccessResponse(null!, "Budget updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Budget not found", new { id = new[] { "Invalid budget ID." } }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            try
            {
                await _service.DeleteAsync(id, userId);
                return Ok(ApiResponse<object>.SuccessResponse(null!, "Budget deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Budget not found", new { id = new[] { "Invalid budget ID." } }));
            }
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);
        }
    }
}
