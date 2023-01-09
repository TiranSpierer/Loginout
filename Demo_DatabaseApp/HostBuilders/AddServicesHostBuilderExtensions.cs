using DataAccess.DataModels;
using DataAccess.Repository;
using DataService.Initialization;
using DataService.Services;
using Loginout.Services;
using Loginout.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Loginout.HostBuilders;

public static class AddServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        host.ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRepository<User>, Repository<User>>();

            services.AddTransient<DbInitializer>();

            services.AddSingleton(hostContext.Configuration);

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddTransient<INavigationService, NavigationService>();
            services.AddSingleton<NavigationStore>();
        });
        return host;
    }
}

