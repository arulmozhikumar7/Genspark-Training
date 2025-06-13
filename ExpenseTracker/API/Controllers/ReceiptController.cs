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
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _service;

        public ReceiptController(IReceiptService service)
        {
            _service = service;
        }

        [HttpPost("upload/{expenseId}")]
        public async Task<IActionResult> Upload(Guid expenseId, IFormFile file)
        {
            var userId = GetUserId();

            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid file upload", new { file = new[] { "File must not be empty." } }));
            }

            try
            {
                await _service.UploadReceiptAsync(userId, expenseId, file);
                return Ok(ApiResponse<object>.SuccessResponse(null!, "Receipt uploaded successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Expense not found", new { expenseId = new[] { "Invalid expense ID." } }));
            }
        }

        [HttpGet("expense/{expenseId}")]
        public async Task<IActionResult> GetReceipts(Guid expenseId)
        {
            var userId = GetUserId();

            try
            {
                var receipts = await _service.GetReceiptsForExpenseAsync(userId, expenseId);
                return Ok(ApiResponse<object>.SuccessResponse(receipts, "Receipts fetched successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Expense not found", new { expenseId = new[] { "Invalid expense ID." } }));
            }
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var userId = GetUserId();

            try
            {
                var (content, contentType, fileName) = await _service.DownloadReceiptAsync(userId, id);
                return File(content, contentType, fileName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Receipt not found", new { id = new[] { "Invalid receipt ID." } }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            try
            {
                await _service.DeleteReceiptAsync(userId, id);
                return Ok(ApiResponse<object>.SuccessResponse(null!, "Receipt deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Receipt not found", new { id = new[] { "Invalid receipt ID." } }));
            }
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);
        }
    }
}
