// Dtos/DoctorBulkCreateDto.cs
namespace HospitalManagementAPI.Dtos
{
    public class DoctorBulkCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new();
    }
}
