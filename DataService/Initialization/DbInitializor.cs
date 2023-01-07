
using DataService.Services;
using Microsoft.Extensions.Configuration;

namespace DataService.Initialization;

public class DbInitializor
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public DbInitializor(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    public async void Initialize()
    {
        string defaultAdminUsername = _configuration["DefaultAdmin:Username"]!;
        string defaultAdminPassword = _configuration["DefaultAdmin:Password"]!;

        // Create the default administrator user if it doesn't already exist
        if (await _userService.AuthenticateAsync(defaultAdminUsername, defaultAdminPassword) == false)
        {
            await _userService.RegisterAsync(defaultAdminUsername, defaultAdminPassword, defaultAdminUsername);
        }
    }
}
