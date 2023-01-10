using System.IO;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo_DatabaseApp.HostBuilders;

public static class AddDbContextHostBuilderExtensions
{
    public static IHostBuilder AddDbContext(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            string connectionString = context.Configuration.GetConnectionString("Default")!;
            Directory.CreateDirectory(Path.GetDirectoryName(connectionString.Replace("Data Source=", ""))!);

            services.AddSingleton(new EnvueDbContextFactory(connectionString!));
            services.AddDbContext<EnvueDbContext>(options => options.UseSqlite(connectionString));
        });

        return host;
    }
}
