using HospitalManagementAPI.Data;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementAPI.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly HospitalDbContext _context;

        public ReportRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<Report> AddAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<IEnumerable<Report>> GetReportsByPatientIdAsync(int patientId)
        {
            return await _context.Reports
                .Where(r => r.PatientId == patientId)
                .ToListAsync();
        }
    }
}
