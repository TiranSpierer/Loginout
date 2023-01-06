using Loginout.HostBuilders;
using Loginout.ViewModels;
using Microsoft.Extensions.Configuration;
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
            .ConfigureServices((hostContext, services) =>
            {
                string connectionString = hostContext.Configuration.GetConnectionString("Default");

                services.AddSingleton(new ENvueDbContextFactory(connectionString));
                services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
                services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
                services.AddSingleton<IReservationConflictValidator, DatabaseReservationConflictValidator>();

                services.AddTransient<ReservationBook>();

                string hotelName = hostContext.Configuration.GetValue<string>("HotelName");
                services.AddSingleton((s) => new Hotel(hotelName, s.GetRequiredService<ReservationBook>()));

                services.AddSingleton<HotelStore>();
                services.AddSingleton<NavigationStore>();

                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        ENvueDbContextFactory reservoomDbContextFactory = _host.Services.GetRequiredService<ENvueDbContextFactory>();
        using (ENvueDbContext dbContext = reservoomDbContextFactory.CreateDbContext())
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