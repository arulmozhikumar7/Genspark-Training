namespace HospitalManagementAPI.Dtos
{
   
    public class DoctorCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new();
    }

 
}
