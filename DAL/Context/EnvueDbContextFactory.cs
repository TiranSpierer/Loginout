using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context;

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
