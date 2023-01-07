

using DAL.Context;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loginout.HostBuilders;

public static class AddDbContextHostBuilderExtensions
{
    public static IHostBuilder AddDbContext(this IHostBuilder host)
    {
        host.ConfigureServices((context, services) =>
        {
            string connectionString = context.Configuration.GetConnectionString("Default")!;
            services.AddSingleton(new EnvueDbContextFactory(connectionString!));
        });

        return host;
    }
}
