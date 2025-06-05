using System.ComponentModel.DataAnnotations;

namespace HospitalManagementAPI.Models
{
    public class FreeMedicalCamp
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

        [Required]
        public TimePeriod TimePeriod { get; set; } = default!;

        public string ContactInfo { get; set; } = string.Empty;
    }
}
