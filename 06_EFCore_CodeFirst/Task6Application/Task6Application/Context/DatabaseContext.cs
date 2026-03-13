using Microsoft.EntityFrameworkCore;
using Task6Application.EfConfigurations;
using Task6Application.Models;

namespace Task6Application.Context;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctor { get; set; }
    public DbSet<Patient> Patient { get; set; }
    public DbSet<Prescription> Prescription { get; set; }
    public DbSet<Medicament> Medicament { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }
    public DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MedicamentEfConfiguration());
        modelBuilder.ApplyConfiguration(new PrescriptionMedicamentEfConfiguration());

        modelBuilder.ApplyConfiguration(new PrescriptionEfConfiguration());

        modelBuilder.ApplyConfiguration(new DoctorEfConfiguration());
        modelBuilder.ApplyConfiguration(new PatientEfConfiguration());
            
        base.OnModelCreating(modelBuilder);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApbdDbContext).Assembly);
    }
}