using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "User")]
    public class TourProgressController : ControllerBase
    {
        private readonly ITourProgressService _tourProgressService;

        public TourProgressController(ITourProgressService tourProgressService)
        {
            _tourProgressService = tourProgressService;
        }

        [HttpGet]
        public async Task<ActionResult<TourProgress>> GetProgress()
        {
            var userId = GetUserId();
            var progress = await _tourProgressService.GetProgressAsync(userId);
            return Ok(ApiResponse<object>.SuccessResponse(progress, "Tour progress fetched successfully"));
        }

        [HttpPost("complete/{tourKey}")]
        public async Task<ActionResult<TourProgress>> MarkTourAsCompleted(string tourKey)
        {
            if (string.IsNullOrWhiteSpace(tourKey))
                return BadRequest(ApiResponse<object>.ErrorResponse("Tour key must be provided."));

            var userId = GetUserId();
            var updatedProgress = await _tourProgressService.MarkTourAsCompletedAsync(userId, tourKey);
            return Ok(ApiResponse<object>.SuccessResponse(updatedProgress, "Tour marked as completed"));
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }
}
