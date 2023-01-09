using DataAccess.Context;
using DataService.Initialization;
using Loginout.HostBuilders;
using Loginout.Services;
using Loginout.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Loginout;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .AddViewModels()
            .AddViews()
            .AddDbContext()
            .AddServices()
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        EnvueDbContextFactory envueDbContextFactory = _host.Services.GetRequiredService<EnvueDbContextFactory>();
        using (EnvueDbContext dbContext = envueDbContextFactory.CreateDbContext())
        {
            dbContext.Database.Migrate();
        }

        _host.Services.GetRequiredService<DbInitializer>().Initialize();

        NavigationService navigationService = _host.Services.GetRequiredService<NavigationService>();
        navigationService.Navigate(typeof(LoginViewModel));

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.Dispose();

        base.OnExit(e);
    }
}