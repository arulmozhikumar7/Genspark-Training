using System;

namespace NotifyAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string UploadedBy { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
