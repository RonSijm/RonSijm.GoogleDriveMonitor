using RonSijm.GoogleDriveMonitor.CoreLib.Extensions;
using RonSijm.GoogleDriveMonitor.CoreLib.Processors;
using RonSijm.GoogleDriveMonitor.DAL.Entities;

namespace RonSijm.GoogleDriveMonitor.CoreLib;

public class GoogleDriveFacade(ILogger<GoogleDriveFacade> logger, GoogleDriveServiceFactory googleDriveServiceFactory, LocalDataContext localDataContext, GoogleDriveFileProcessor processor)
{
    public async Task Start(CancellationToken cancellationToken)
    {
        var service = await googleDriveServiceFactory.CreateGoogleService();

        await PostProcessPreviousProcesses(cancellationToken);

        var process = localDataContext.Add(new ProcessEntity { Started = DateTime.UtcNow, Status = StatusType.New });
        await localDataContext.SaveChangesAsync(cancellationToken);

        var lastModified = GetLastModifiedLocally();
        
        string listFilesPageToken = null;

        do
        {
            var token = listFilesPageToken;

            var listFiles = await service.Files.List()
                .With(x => x.PageSize = 20)
                .With(x => x.Fields = "*")
                .With(x => x.PageToken = token)
                .With(x => x.IncludeItemsFromAllDrives = true)
                .With(x => x.SupportsAllDrives = true)
                .With(x => x.OrderBy = "modifiedTime desc")
                .ExecuteAsync(cancellationToken);

            listFilesPageToken = listFiles.NextPageToken;

            var files = listFiles.Files;

            if (files is { Count: > 0 })
            {
                foreach (var file in files)
                {
                    if (lastModified >= file.ModifiedTimeDateTimeOffset)
                    {
                        // Since we already have this modification locally, we've reached the end.
                        listFilesPageToken = null;
                        break;
                    }

                    file.ProcessId = process.Entity.Id;
                    await processor.ProcessFiles(file, service, cancellationToken);
                }
            }
            else
            {
                logger.LogInformation("No files found.");
            }

        } while (listFilesPageToken != null);

        process.Entity.Status = StatusType.Finished;
        process.Entity.Finished = DateTime.UtcNow;

        await localDataContext.SaveChangesAsync(cancellationToken);
    }

    private DateTimeOffset GetLastModifiedLocally()
    {
        var lastModified = DateTimeOffset.MinValue;

        var lastModifiedFile = localDataContext.File.OrderByDescending(x => x.ModifiedTime).FirstOrDefault();

        if (lastModifiedFile is { ModifiedTimeDateTimeOffset: not null })
        {
            lastModified = lastModifiedFile.ModifiedTimeDateTimeOffset.Value;

            logger.LogInformation("Checking for changes since last known modification at {lastModified}", lastModified);
        }

        return lastModified;
    }

    private async Task PostProcessPreviousProcesses(CancellationToken cancellationToken)
    {
        ProcessEntity previousProcess;

        do
        {
            previousProcess = localDataContext.Process.OrderByDescending(x => x.Finished).FirstOrDefault();

            if (previousProcess == null)
            {
                continue;
            }

            if (previousProcess.Status != StatusType.New)
            {
                continue;
            }

            logger.LogWarning("Warning: Previous processing from {startDate} did not complete successfuly.", previousProcess.Started);

            // The previous process stopped while processing, so the status is not reliable.
            localDataContext.Process.Remove(previousProcess);
            await localDataContext.SaveChangesAsync(cancellationToken);

        } while (previousProcess is { Status: StatusType.New });
    }
}