using Demo_DatabaseApp.ViewModels.Surviews;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo_DatabaseApp.HostBuilders;

public static class AddViewsHostBuilderExtensions
{
    public static IHostBuilder AddViews(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
        });

        return host;
    }
}
