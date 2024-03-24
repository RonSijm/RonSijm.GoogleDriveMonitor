using RonSijm.GoogleDriveMonitor.CoreLib.Extensions;

namespace RonSijm.GoogleDriveMonitor.CoreLib.Processors;

// ReSharper disable once ClassNeverInstantiated.Global - Justification: Used by DI
public class GoogleDriveChangeProcessor(ILogger<GoogleDriveChangeProcessor> logger)
{
    public async Task FetchAndProcessChanges(DriveService service, CancellationToken cancellationToken)
    {
        var startPageToken = await service.Changes.GetStartPageToken().ExecuteAsync(cancellationToken);
        var changesStartPageToken = startPageToken.StartPageTokenValue;

        var changesRequest = service.Changes.List(changesStartPageToken).With(x => x.Fields = "*");

        do
        {
            var changesResponse = await changesRequest.ExecuteAsync(cancellationToken);

            if (changesResponse.Changes is { Count: > 0 })
            {
                foreach (var change in changesResponse.Changes)
                {
                    logger.LogInformation("Change Type: {change.ChangeType} Time: {change.TimeDateTimeOffset}", change.ChangeType, change.TimeDateTimeOffset);
                }
            }

            changesStartPageToken = changesResponse.NextPageToken;
        } while (changesStartPageToken != null);
    }
}