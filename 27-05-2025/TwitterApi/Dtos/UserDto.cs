using System;
using TwitterAPI.Models;

namespace TwitterAPI.Dtos
{
    public record UserDto(User user)
    {
        public int Id { get; init; } = user.Id;
        public string Username { get; init; } = user.Username;
        public string Email { get; init; } = user.Email;
        public string? Bio { get; init; } = user.Bio;
        public DateTime CreatedAt { get; init; } = user.CreatedAt;
    }
}
