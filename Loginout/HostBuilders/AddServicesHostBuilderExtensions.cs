using DAL.DataModels;
using DAL.Repository;
using DataService.Services;
using Loginout.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Loginout.HostBuilders;

public static class AddServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRepository<User>, Repository<User>>();

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddSingleton<NavigationStore>();
        });
        return host;
    }
}

