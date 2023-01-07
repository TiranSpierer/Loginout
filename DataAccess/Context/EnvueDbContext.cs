using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class EnvueDbContext : DbContext
{
	public EnvueDbContext(DbContextOptions options) : base(options) 
    {
        
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserBuilder(modelBuilder);
    }

    private static void UserBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
    }
}
