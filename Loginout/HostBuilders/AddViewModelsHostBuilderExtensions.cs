using Loginout.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Loginout.HostBuilders;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<NavigationService<HomeViewModel>>();

            services.AddTransient<LoginViewModel>();
            services.AddSingleton<NavigationService<LoginViewModel>>();

            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<NavigationService<RegisterViewModel>>();

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }
}
