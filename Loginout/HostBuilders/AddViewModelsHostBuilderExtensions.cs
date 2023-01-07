using Loginout.Services;
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
            services.AddTransient<HomeViewModel>();
            services.AddSingleton<NavigationService<HomeViewModel>>();
            services.AddSingleton<Func<HomeViewModel>>((s) => () => s.GetRequiredService<HomeViewModel>());

            services.AddTransient<LoginViewModel>();
            services.AddSingleton<NavigationService<LoginViewModel>>();
            services.AddSingleton<Func<LoginViewModel>>((s) => () => s.GetRequiredService<LoginViewModel>());

            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<NavigationService<RegisterViewModel>>();
            services.AddSingleton<Func<RegisterViewModel>>((s) => () => s.GetRequiredService<RegisterViewModel>());

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }
}
