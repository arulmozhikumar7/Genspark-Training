using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementAPI.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<FreeMedicalCamp> FreeMedicalCamps { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(appointment =>
            {
                appointment.OwnsOne(a => a.TimePeriod);
            });
            modelBuilder.Entity<FreeMedicalCamp>(freemedicalcamp =>
            {
                freemedicalcamp.OwnsOne(fmc => fmc.TimePeriod);
            });
            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });

            modelBuilder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecializations)
                .HasForeignKey(ds => ds.DoctorId);

            modelBuilder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Specialization)
                .WithMany(s => s.DoctorSpecializations)
                .HasForeignKey(ds => ds.SpecializationId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Reports)
                .WithOne(r => r.Patient)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<User>(u => u.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }

    }
}
