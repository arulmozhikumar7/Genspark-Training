
namespace FirstApi.Models{
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Disease { get; set; } = string.Empty;
}
}