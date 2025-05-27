using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Dtos
{
    public record UpdateUserDto
    {
        [MaxLength(100), EmailAddress]
        public string? Email { get; init; }

        public string? Bio { get; init; }
    }
}
