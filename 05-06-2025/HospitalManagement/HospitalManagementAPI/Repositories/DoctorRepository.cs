using HospitalManagementAPI.Data;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;

namespace HospitalManagementAPI.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalDbContext _context;

        public DoctorRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Doctors.AnyAsync(d => d.Id == id && d.IsActive);
        }

        public async Task<IEnumerable<Doctor>> GetAllActiveAsync()
        {
            return await _context.Doctors
                .Include(d => d.DoctorSpecializations)
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.DoctorSpecializations)
                    .ThenInclude(ds => ds.Specialization)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }
        
        public async Task BulkInsertDoctorsAsync(List<DoctorBulkCreateDto> doctors)
        {
            var json = JsonSerializer.Serialize(doctors);
            var parameter = new NpgsqlParameter("p0", NpgsqlDbType.Json)
            {
                Value = json
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
            var sql = "CALL sp_bulk_add_doctors(@p0);";
            await _context.Database.ExecuteSqlRawAsync(sql, parameter);
            await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error during bulk insert: {ex.Message}");
                throw; 
            }
        }


    }
}
