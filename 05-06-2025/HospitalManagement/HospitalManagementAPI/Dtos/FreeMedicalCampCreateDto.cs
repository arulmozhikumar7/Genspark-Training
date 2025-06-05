using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.DTOs
{
    public class FreeMedicalCampCreateDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimePeriod Time { get; set; }
        public string Location { get; set; }
        public int DoctorId { get; set; }
        public string Description { get; set; }
    }
}
