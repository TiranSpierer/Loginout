using DataAccess.DataModels;
using DataAccess.Repository;
using DataService.Initialization;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo_DatabaseApp.HostBuilders;

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

