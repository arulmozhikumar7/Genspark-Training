using System;

namespace HospitalManagementAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }  
        public TimePeriod TimePeriod { get; set; } = null!;  

        public string DoctorName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public bool IsCancelled { get; set; } = false;

        // Navigation properties
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
