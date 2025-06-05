using System.Collections.Generic;

namespace HospitalManagementAPI.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; } = new List<DoctorSpecialization>();

    }
}