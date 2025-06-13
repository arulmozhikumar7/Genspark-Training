namespace ExpenseTrackerAPI.DTOs
{
    public class PaginatedExpenseResponseDto
    {
        public IEnumerable<ExpenseResponseDto> Items { get; set; } = Enumerable.Empty<ExpenseResponseDto>();
        public int TotalCount { get; set; }
    }
}
