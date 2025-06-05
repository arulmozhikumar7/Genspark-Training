using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> AddAsync(Report report);
        Task<IEnumerable<Report>> GetReportsByPatientIdAsync(int patientId);
    }
}
