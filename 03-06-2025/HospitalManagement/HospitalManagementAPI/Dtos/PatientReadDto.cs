namespace HospitalManagementAPI.Dtos
{
    public class PatientReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
