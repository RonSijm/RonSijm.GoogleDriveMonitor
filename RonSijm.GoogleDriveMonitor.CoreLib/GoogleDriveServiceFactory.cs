using System.IO;

using RonSijm.GoogleDriveMonitor.CoreLib.Auth;

namespace RonSijm.GoogleDriveMonitor.CoreLib;

// ReSharper disable once ClassNeverInstantiated.Global - Justification: Used by DI
public class GoogleDriveServiceFactory(DatabaseBasedGoogleDataStore databaseBasedGoogleDataStore)
{
    private static readonly string[] Scopes = [DriveService.Scope.DriveReadonly];
    private static readonly string ApplicationName = "Appical AI Google Drive Explorer";

    public async Task<DriveService> CreateGoogleService()
    {
        UserCredential credential;

        await using (var stream = new FileStream("Credentials.json", FileMode.Open, FileAccess.Read))
        {
            var googleClientSecrets = await GoogleClientSecrets.FromStreamAsync(stream);
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(googleClientSecrets.Secrets, Scopes, "user", CancellationToken.None, databaseBasedGoogleDataStore);
        }

        var service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        return service;
    }
}