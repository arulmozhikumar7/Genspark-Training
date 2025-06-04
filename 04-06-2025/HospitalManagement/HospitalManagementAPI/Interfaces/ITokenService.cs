using HospitalManagementAPI.Models;
using Google.Apis.Auth;

namespace HospitalManagementAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);

    }
}
