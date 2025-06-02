using HospitalManagementAPI.DTOs;
using HospitalManagementAPI.Models;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(RegisterDto request);
        Task<ServiceResponse<string>> Login(LoginDto request);
    }
}
