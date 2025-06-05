using HospitalManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("{patientId}")]
        public async Task<IActionResult> UploadReport(IFormFile file, int patientId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            var report = await _reportService.UploadReportAsync(file, patientId);
            return Ok(report);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetReports(int patientId)
        {
            var reports = await _reportService.GetReportsAsync(patientId);
            return Ok(reports);
        }
    }
}
