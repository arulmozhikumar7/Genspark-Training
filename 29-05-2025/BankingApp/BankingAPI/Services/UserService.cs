using System.Threading.Tasks;
using BankingAPI.Interfaces;
using BankingAPI.Mappers;
using BankingAPI.DTOs;
using BankingAPI.Models;

namespace BankingAPI.Services
{
    public class UserService
    {
        private readonly IUser _userRepository;

        public UserService(IUser userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto dto)
        {
            var user = dto.ToUser();
            var createdUser = await _userRepository.AddUserAsync(user);
            return createdUser.ToUserReadDto();
        }
    }
}
