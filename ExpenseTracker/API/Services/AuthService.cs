using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Utilities;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly TokenHelper _tokenHelper;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepo, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _refreshTokenRepo = refreshTokenRepo;
            _tokenHelper = tokenHelper;
        }

        public async Task<AuthResponse?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            var token = _tokenHelper.GenerateJwtToken(user);
            var refreshToken = _tokenHelper.CreateRefreshToken("127.0.0.1");

            await _refreshTokenRepo.SaveRefreshTokenAsync(user.Id, refreshToken);

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                UserId = user.Id,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }
        public async Task<AuthResponse?> RefreshAccessTokenAsync(string token, string refreshTokenValue)
        {
            var principal = _tokenHelper.GetPrincipalFromExpiredToken(token);
            if (principal == null)
                return null;

            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            Console.WriteLine($"Extracted UserId from token: {userId}");

            var storedToken = await _refreshTokenRepo.GetRefreshTokenAsync(userId);

            if (storedToken == null)
            {
                Console.WriteLine("Stored token not found.");
                return null;
            }

            if (storedToken.Token != refreshTokenValue)
            {
                return null;
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var newJwt = _tokenHelper.GenerateJwtToken(user);
            var newRefresh = _tokenHelper.CreateRefreshToken("127.0.0.1");

            await _refreshTokenRepo.UpdateRefreshTokenAsync(user.Id, newRefresh);

            return new AuthResponse
            {
                Token = newJwt,
                RefreshToken = newRefresh.Token,
                UserId = user.Id,
                UserName = user.UserName,
                Role = user.Role.ToString()
            };
        }

        public async Task LogoutAsync(Guid userId, string refreshToken)
        {
            var storedToken = await _refreshTokenRepo.GetRefreshTokenAsync(userId);

            if (storedToken != null && storedToken.Token == refreshToken)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                storedToken.RevokedByIp = "127.0.0.1";
                await _refreshTokenRepo.RevokeTokenAsync(storedToken);
            }
        }
        
        public ClaimsPrincipal? ValidateToken(string token)
        {
            return _tokenHelper.GetPrincipalFromExpiredToken(token);
        }


    }
}
