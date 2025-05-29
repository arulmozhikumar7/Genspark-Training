using BankingAPI.DTOs;
using BankingAPI.Models;

namespace BankingAPI.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this UserCreateDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public static UserReadDto ToUserReadDto(this User user)
        {
            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
