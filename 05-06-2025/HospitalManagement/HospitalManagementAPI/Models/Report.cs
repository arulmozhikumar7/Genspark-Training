using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementAPI.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }
    }
}
