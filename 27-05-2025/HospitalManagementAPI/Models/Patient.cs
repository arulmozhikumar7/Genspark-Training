using System.Collections.Generic;

namespace HospitalManagementAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<Appointment> Appointments { get; set; } = new();
    }
}