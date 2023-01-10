using System.Windows;
using DataAccess.Context;
using DataService.Initialization;
using Demo_DatabaseApp.HostBuilders;
using Demo_DatabaseApp.Services;
using Demo_DatabaseApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo_DatabaseApp;

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

        INavigationService navigationService = _host.Services.GetRequiredService<INavigationService>();
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