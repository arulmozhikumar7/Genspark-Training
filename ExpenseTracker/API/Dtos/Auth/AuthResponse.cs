namespace ExpenseTrackerAPI.DTOs
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
