using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NotifyAPI.Interfaces;
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.', 2);
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        var hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));

        return hash == hashedInput;
    }
}
