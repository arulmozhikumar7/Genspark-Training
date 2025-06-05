using NotifyAPI.Models;
using NotifyAPI.Interfaces;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null) return null;

        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            return null;

        return _tokenService.GenerateToken(user);
    }

    public async Task<User> RegisterAsync(User user, string password)
    {
        user.PasswordHash = _passwordHasher.HashPassword(password);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return user;
    }
}
