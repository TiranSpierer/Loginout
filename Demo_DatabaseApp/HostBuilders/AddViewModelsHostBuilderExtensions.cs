using Demo_DatabaseApp.ViewModels.Subviews;
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
            services.AddSingleton<MainViewModel>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<EditViewModel>();

            services.AddTransient<HomeSubOneViewModel>();
            services.AddTransient<HomeSubTwoViewModel>();
        });

        return hostBuilder;
    }


}
