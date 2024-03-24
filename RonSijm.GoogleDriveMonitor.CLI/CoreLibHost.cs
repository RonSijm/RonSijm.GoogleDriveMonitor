using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RonSijm.GoogleDriveMonitor.CoreLib;

namespace RonSijm.GoogleDriveMonitor.CLI;

public class CoreLibHost(HostSettings hostSettings, ILogger<CoreLibHost> logger, GoogleDriveFacade googleDriveFacade) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting...");
        await googleDriveFacade.Start(cancellationToken);

        logger.LogInformation("Finished!");

        if (!hostSettings.Silent)
        {
            Console.Read();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Do Nothing
        return Task.CompletedTask;
    }
}