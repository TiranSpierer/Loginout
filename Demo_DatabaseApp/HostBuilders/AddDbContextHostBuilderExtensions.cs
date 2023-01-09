

using DataAccess.Context;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows.Markup;

namespace Loginout.HostBuilders;

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
