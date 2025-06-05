using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.AspNetCore.Hosting; 
namespace HospitalManagementAPI.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repository;
        private readonly IWebHostEnvironment _environment;

        public ReportService(IReportRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        public async Task<Report> UploadReportAsync(IFormFile file, int patientId)
        {
            var reportsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "Reports");

    
            if (!Directory.Exists(reportsFolder))
                Directory.CreateDirectory(reportsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(reportsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var report = new Report
            {
                FileName = file.FileName,
                FilePath = $"/Reports/{fileName}",
                UploadedAt = DateTime.UtcNow,
                PatientId = patientId
            };

            return await _repository.AddAsync(report);
        }

        public async Task<IEnumerable<Report>> GetReportsAsync(int patientId)
        {
            return await _repository.GetReportsByPatientIdAsync(patientId);
        }
    }
}
