
namespace HospitalManagementAPI.Dtos{
public class DoctorReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SpecializationId { get; set; }
    public bool IsActive { get; set; }
    public string? SpecializationName { get; set; }
}
}
