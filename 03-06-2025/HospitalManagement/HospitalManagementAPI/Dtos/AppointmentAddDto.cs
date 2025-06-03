
namespace HospitalManagementAPI.Dtos
{
    public class AppointmentAddDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
