using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "User")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpenseController(IExpenseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                var errors = new Dictionary<string, string[]>();

                if (page < 1)
                    errors["page"] = new[] { "Page number must be at least 1." };

                if (pageSize < 1 || pageSize > 100)
                    errors["pageSize"] = new[] { "Page size must be between 1 and 100." };

                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid pagination parameters", errors));
            }

            var userId = GetUserId();
            var result = await _service.GetAllAsync(userId, page, pageSize);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Expenses fetched successfully"));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = GetUserId();

            try
            {
                var expense = await _service.GetByIdAsync(id, userId);
                return Ok(ApiResponse<object>.SuccessResponse(expense, "Expense fetched successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Expense not found", new { id = new[] { "Invalid expense ID." } }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseCreateDto dto)
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
            return Ok(ApiResponse<object>.SuccessResponse(null!, "Expense created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExpenseUpdateDto dto)
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
                return Ok(ApiResponse<object>.SuccessResponse(null!, "Expense updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Expense not found", new { id = new[] { "Expense with the specified ID does not exist." } }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            try
            {
                await _service.DeleteAsync(id, userId);
                return Ok(ApiResponse<object>.SuccessResponse(null!, "Expense deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Expense not found", new { id = new[] { "Expense with the specified ID does not exist." } }));
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered([FromQuery] ExpenseQueryParameters parameters)
        {
            var userId = GetUserId();
            var result = await _service.GetFilteredAsync(userId, parameters);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Filtered expenses fetched successfully"));
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportToCsv([FromQuery] ExpenseQueryParameters parameters)
        {
            var userId = GetUserId();
            var csv = await _service.ExportCsvAsync(userId, parameters);

            var bytes = Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"expenses_{DateTime.UtcNow:yyyyMMdd}.csv");
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value!);
        }
    }
}
