using Demo_DatabaseApp.ViewModels.Surviews;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo_DatabaseApp.HostBuilders;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<HomeViewModel>();
            //services.AddSingleton<NavigationService<HomeViewModel>>();
            //services.AddSingleton<Func<HomeViewModel>>((s) => s.GetRequiredService<HomeViewModel>);

            services.AddTransient<LoginViewModel>();
            //services.AddSingleton<NavigationService<LoginViewModel>>();
            //services.AddSingleton<Func<LoginViewModel>>((s) => s.GetRequiredService<LoginViewModel>);

            services.AddTransient<RegisterViewModel>();
            //services.AddSingleton<NavigationService<RegisterViewModel>>();
            //services.AddSingleton<Func<RegisterViewModel>>((s) => s.GetRequiredService<RegisterViewModel>);

            services.AddTransient<EditViewModel>();
            //services.AddSingleton<NavigationService<EditViewModel>>();
            //services.AddSingleton<Func<EditViewModel>>((s) => s.GetRequiredService<EditViewModel>);

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }
}
