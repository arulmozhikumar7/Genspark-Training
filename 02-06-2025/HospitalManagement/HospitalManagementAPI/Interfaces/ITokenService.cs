using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
