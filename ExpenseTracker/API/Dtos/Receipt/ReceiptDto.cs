namespace ExpenseTrackerAPI.DTOs
{
    public class ReceiptDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
