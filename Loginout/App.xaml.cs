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

    //public App()
    //{
    //    _host = Host.CreateDefaultBuilder()
    //        .AddViewModels()
    //        .AddViews()
    //        .AddDbContext()
    //        .AddServices()
    //        .Build();
    //}

    public App()
    {
        var host = Host.CreateDefaultBuilder();

        host.ConfigureServices((context, services) =>
        {
            string connectionString = context.Configuration.GetConnectionString("Default")!;
            services.AddSingleton(new EnvueDbContextFactory(connectionString!));
            services.AddDbContext<EnvueDbContext>(options => options.UseSqlite(connectionString));

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRepository<User>, Repository<User>>();

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddSingleton<NavigationStore>();

            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<NavigationService<HomeViewModel>>();
            services.AddSingleton<Func<HomeViewModel>>((s) => () => s.GetRequiredService<HomeViewModel>());

            services.AddTransient<LoginViewModel>();
            services.AddSingleton<NavigationService<LoginViewModel>>();
            services.AddSingleton<Func<LoginViewModel>>((s) => () => s.GetRequiredService<LoginViewModel>());

            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<NavigationService<RegisterViewModel>>();
            services.AddSingleton<Func<RegisterViewModel>>((s) => () => s.GetRequiredService<RegisterViewModel>());

            services.AddSingleton<MainViewModel>();

            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
        });

        _host = host.Build();

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