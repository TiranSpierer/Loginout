using System;
using DataAccess.DataModels;
using DataAccess.Repository;
using DataService.Initialization;
using DataService.Services;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.Stores;
using EventAggregator.ViewModelChanged;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prism.Events;

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
            services.AddSingleton<IEventAggregator, Prism.Events.EventAggregator>();

            services.AddSingleton<NavigationStore<SubViewModelChanged>>();
            services.AddSingleton<NavigationStore<MainViewModelChanged>>();
            services.AddSingleton<INavigationService, NavigationService>();
        });
        return host;
    }

}

