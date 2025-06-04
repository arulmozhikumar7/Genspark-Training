namespace HospitalManagementAPI.DTOs
{
    public class GoogleLoginDto
    {
        public string IdToken { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
