using HospitalManagementAPI.Data;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly HospitalDbContext _context;

        public SpecializationRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Specialization specialization)
        {
            await _context.Specializations.AddAsync(specialization);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            return await _context.Specializations
                            .Include(s => s.DoctorSpecializations)
                                  .ThenInclude(ds => ds.Doctor)  
                            .ToListAsync();
        }

        public async Task<Specialization?> GetByIdAsync(int id)
        {
           return await _context.Specializations
                            .Include(s => s.DoctorSpecializations)
                                .ThenInclude(ds => ds.Doctor)
                            .FirstOrDefaultAsync(s => s.Id == id);
        }

        
    }
}
