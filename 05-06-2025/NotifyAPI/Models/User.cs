namespace NotifyAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        // Roles: "HR" for admins, "User" for regular staff
        public string Role { get; set; } = "User";
    }
}
