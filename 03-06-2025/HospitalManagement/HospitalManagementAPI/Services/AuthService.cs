using HospitalManagementAPI.Data;
using HospitalManagementAPI.DTOs;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HospitalManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly HospitalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionService _encryptionService;
        private readonly ITokenService _tokenService;

        public AuthService(
            HospitalDbContext context,
            IConfiguration configuration,
            IEncryptionService encryptionService,
            ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _encryptionService = encryptionService;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse<string>> Register(RegisterDto request)
        {
            var response = new ServiceResponse<string>();

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                response.Success = false;
                response.Message = "Username already exists.";
                return response;
            }

            // Create password hash and salt
            _encryptionService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = request.Username,
                Role = request.Role,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            response.Data = "User registered successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> Login(LoginDto request)
        {
            var response = new ServiceResponse<string>();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            if (!_encryptionService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
                return response;
            }

            string token = _tokenService.CreateToken(user);
            response.Data = token;
            return response;
        }
    }
}
