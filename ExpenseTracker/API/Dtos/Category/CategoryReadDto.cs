namespace ExpenseTrackerAPI.DTOs
{
    public class CategoryReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
