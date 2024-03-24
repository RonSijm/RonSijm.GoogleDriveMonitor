using RonSijm.GoogleDriveMonitor.CoreLib.Extensions;
using RonSijm.GoogleDriveMonitor.CoreLib.Helpers;
using RonSijm.GoogleDriveMonitor.DAL.Entities;

namespace RonSijm.GoogleDriveMonitor.CoreLib.Processors;

// ReSharper disable once ClassNeverInstantiated.Global - Justification: Used by DI
public class GoogleDriveFileProcessor(ILogger<GoogleDriveFileProcessor> logger, LocalDataContext localDataContext, GoogleDriveFileCommentProcessor commentProcessor, GoogleDriveChangeProcessor changeProcessor)
{
    public async Task ProcessFiles(File file, DriveService service, CancellationToken cancellationToken)
    {
        var existingRecord = localDataContext.File.FirstOrDefault(x => x.Id == file.Id);
        logger.LogFileProcessingStart(file, existingRecord);

        if (existingRecord == null)
        {
            UserManager.FixUsers(file);

            localDataContext.MergeExistingPermissions(file);

            file.StatusType = StatusType.New;
            localDataContext.Add(file);
            await localDataContext.SaveChangesAsync(cancellationToken);

            existingRecord = file;
        }
        // Processor did not finish last run
        else if (existingRecord.StatusType == StatusType.New)
        {

        }
        else
        {
            return;
        }

        await commentProcessor.FetchAndProcessComments(existingRecord, service, cancellationToken);
        await changeProcessor.FetchAndProcessChanges(service, cancellationToken);

        existingRecord.StatusType = StatusType.Finished;
        await localDataContext.SaveChangesAsync(cancellationToken);
    }
}