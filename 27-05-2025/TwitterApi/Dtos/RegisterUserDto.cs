using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Dtos
{
    public record RegisterUserDto
    {
        [Required, MaxLength(50)]
        public string Username { get; init; } = null!;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; init; } = null!;

        [Required, MinLength(6)]
        public string Password { get; init; } = null!;

        public string? Bio { get; init; }
    }
}
