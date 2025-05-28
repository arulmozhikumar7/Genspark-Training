using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.Dtos{
    public class AppointmentReadDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimePeriod TimePeriod { get; set; } = null!;
        public string Reason { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
    }

}