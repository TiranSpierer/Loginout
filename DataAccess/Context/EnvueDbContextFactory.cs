using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class EnvueDbContextFactory
{
    private readonly string _connectionString;

    public EnvueDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public EnvueDbContext CreateDbContext()
    {
        DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

        return new EnvueDbContext(options);
    }

}
