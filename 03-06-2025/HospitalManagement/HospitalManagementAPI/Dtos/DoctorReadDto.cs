
namespace HospitalManagementAPI.Dtos{
    public class DoctorReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string> SpecializationNames { get; set; } = new();
    }

}
