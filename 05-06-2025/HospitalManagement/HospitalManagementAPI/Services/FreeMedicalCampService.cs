using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.AspNetCore.SignalR;
using HospitalManagementAPI.Hubs;
namespace HospitalManagementAPI.Services
{
    public class FreeMedicalCampService
    {
        private readonly IFreeMedicalCampRepository _repository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public FreeMedicalCampService(IFreeMedicalCampRepository repository, IHubContext<NotificationHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext; 
        }

       public async Task<FreeMedicalCamp> CreateCampAsync(FreeMedicalCamp camp)
{
    var addedCamp = await _repository.AddAsync(camp);

    var campDto = new
    {
        title = addedCamp.Title,
        doctorName = addedCamp.Doctor?.Name ?? "Unknown"
    };

    await _hubContext.Clients.All.SendAsync("NewCampAdded", campDto);
    Console.WriteLine($"Notification sent for camp: {addedCamp.Title}");
    return addedCamp;
}


        public Task<IEnumerable<FreeMedicalCamp>> GetAllCampsAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<FreeMedicalCamp?> GetCampByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<IEnumerable<FreeMedicalCamp>> GetCampsByDoctorAsync(int doctorId)
        {
            return _repository.GetByDoctorIdAsync(doctorId);
        }

        public Task<bool> DeleteCampAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
