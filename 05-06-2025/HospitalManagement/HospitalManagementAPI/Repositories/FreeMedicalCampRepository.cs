using HospitalManagementAPI.Data;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementAPI.Repositories
{
    public class FreeMedicalCampRepository : IFreeMedicalCampRepository
    {
        private readonly HospitalDbContext _context;

        public FreeMedicalCampRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<FreeMedicalCamp> AddAsync(FreeMedicalCamp camp)
        {
            _context.FreeMedicalCamps.Add(camp);
            await _context.SaveChangesAsync();
            return camp;
        }

        public async Task<IEnumerable<FreeMedicalCamp>> GetAllAsync()
        {
            return await _context.FreeMedicalCamps.Include(c => c.Doctor).ToListAsync();
        }

        public async Task<FreeMedicalCamp?> GetByIdAsync(int id)
        {
            return await _context.FreeMedicalCamps.Include(c => c.Doctor).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<FreeMedicalCamp>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.FreeMedicalCamps.Where(c => c.DoctorId == doctorId).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var camp = await _context.FreeMedicalCamps.FindAsync(id);
            if (camp == null) return false;

            _context.FreeMedicalCamps.Remove(camp);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
