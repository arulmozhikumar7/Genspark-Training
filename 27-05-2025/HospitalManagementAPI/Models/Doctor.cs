using System.Collections.Generic;

namespace HospitalManagementAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SpecializationId { get; set; }
        public bool IsActive { get; set; } = true;

        public Specialization? Specialization { get; set; }
        public List<Appointment>? Appointments { get; set; } 
    }
}