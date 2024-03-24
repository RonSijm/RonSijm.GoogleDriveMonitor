using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RonSijm.GoogleDriveMonitor.CoreLib;
using RonSijm.GoogleDriveMonitor.DAL;

namespace RonSijm.GoogleDriveMonitor.CLI;

// ReSharper disable once ClassNeverInstantiated.Global - Justification: Main entry point
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var googleAccountSettings = new GoogleAccountSettings();
        configuration.GetSection("GoogleAccountSettings").Bind(googleAccountSettings);

        if (string.IsNullOrWhiteSpace(googleAccountSettings.AccountToUse))
        {
            throw new ArgumentNullException(nameof(googleAccountSettings.AccountToUse));
        }

        if (string.IsNullOrWhiteSpace(googleAccountSettings.StorageLocation))
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\net8.0\\", string.Empty);
            googleAccountSettings.StorageLocation = currentDir + "\\Data";
        }

        var hostSettings = new HostSettings
        {
            Silent = args != null && args.Any(x => x.Contains("silent", StringComparison.InvariantCultureIgnoreCase))
        };

        SilentModeHelper.HideHostIfSilentMode(hostSettings.Silent);

        // Build DI Container, and start the app.
        var hostBuilder = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
        {
            services.AddSingleton(googleAccountSettings);
            services.AddSingleton(hostSettings);

            services.AddTypesAndInterfaces(typeof(GoogleDriveFacade));
            services.AddSingleton(typeof(LocalDataContext));

            services.AddHostedService<CoreLibHost>();
        });

        var host = hostBuilder.Build();

        await InitDatabase(host);

        await host.RunAsync();

        return 1;
    }

    private static async Task InitDatabase(IHost host)
    {
        var database = host.Services.GetRequiredService<LocalDataContext>();
        database.EnsureCreated();
        await database.Database.ExecuteSqlRawAsync("VACUUM;");
        var users = database.User.ToDictionary(x => x.GetIdentifier());
        UserManager.Init(users);
    }
}