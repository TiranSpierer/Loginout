using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class EnvueDbContext : DbContext
{
	public EnvueDbContext(DbContextOptions options) : base(options) 
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Patient>? Patients { get; set; }
    public DbSet<UserPrivilege>? UserPrivileges { get; set; }
    public DbSet<Registration>? Registrations { get; set; }
    public DbSet<Procedure>? Procedures { get; set; }
    public DbSet<Frame>? Frames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserBuilder(modelBuilder);
        PatientBuilder(modelBuilder);
        UserPrivilegeBuilder(modelBuilder);
        RegistrationBuilder(modelBuilder);
        ProcedureBuilder(modelBuilder);
        FrameBuilder(modelBuilder);
    }

    private static void UserBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .HasMany(u => u.UserPrivileges)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
    }

    private static void PatientBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
                    .Property(p => p.Id)
                    .ValueGeneratedNever();

        modelBuilder.Entity<Patient>()
                    .Property(p => p.Name)
                    .IsRequired(false);

        modelBuilder.Entity<Patient>()
                    .Property(p => p.DateOfBirth)
                    .IsRequired(false);
    }

    private static void UserPrivilegeBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPrivilege>()
                    .HasKey(up => new { up.UserId, up.Privilege });
    }

    private static void RegistrationBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registration>()
                    .HasKey(r => r.ProcedureId);
    }

    private void ProcedureBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Procedure>()
                    .HasOne(p => p.Patient)
                    .WithMany(p => p.Procedures)
                    .HasForeignKey(p => p.PatientId);

        modelBuilder.Entity<Procedure>()
                    .HasOne(p => p.User)
                    .WithMany(u => u.Procedures)
                    .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Procedure>()
                    .HasMany(p => p.Frames)
                    .WithOne(f => f.Procedure)
                    .HasForeignKey(f => f.ProcedureId);
    }

    private void FrameBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Frame>()
                    .HasOne(f => f.Procedure)
                    .WithMany(p => p.Frames)
                    .HasForeignKey(f => f.ProcedureId);
    }
}
