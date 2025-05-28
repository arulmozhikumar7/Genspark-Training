using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementAPI.Models
{
    public class Doctor
    {   
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SpecializationId { get; set; }
        public bool IsActive { get; set; } = true;

        public Specialization? Specialization { get; set; }
        public List<Appointment>? Appointments { get; set; } 
    }
}