using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<CategoryReadDto>>.SuccessResponse(result, "Categories fetched successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found", new
                {
                    Id = new[] { "Category with the specified ID does not exist." }
                }));
            }

            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(category, "Category fetched successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id },
                    ApiResponse<CategoryReadDto>.SuccessResponse(created, "Category created successfully"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "Category name already exists")
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { "Name", new[] { "Category name already exists." } }
                    };
                    return Conflict(ApiResponse<object>.ErrorResponse("Conflict", errors));
                }

                return StatusCode(500, ApiResponse<object>.ErrorResponse("An unexpected error occurred", new
                {
                    Error = new[] { ex.Message }
                }));
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Category not found", new
                    {
                        Id = new[] { "Category with the specified ID does not exist." }
                    }));
                }

                return Ok(ApiResponse<object>.SuccessResponse(null, "Category updated successfully"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "Category name already exists")
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { "Name", new[] { "Category name already exists." } }
                    };
                    return Conflict(ApiResponse<object>.ErrorResponse("Conflict", errors));
                }

                return StatusCode(500, ApiResponse<object>.ErrorResponse("An unexpected error occurred", new
                {
                    Error = new[] { ex.Message }
                }));
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found", new
                {
                    Id = new[] { "Category with the specified ID does not exist." }
                }));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Category deleted successfully"));
        }
    }
}
