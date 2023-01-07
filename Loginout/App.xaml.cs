using DAL.Context;
using DAL.DataModels;
using DAL.Repository;
using DataService.Services;
using Loginout.HostBuilders;
using Loginout.Services;
using Loginout.Stores;
using Loginout.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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

        NavigationService<LoginViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
        navigationService.Navigate();

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