namespace RonSijm.GoogleDriveMonitor.CoreLib.Helpers;

public static class PermissionsMergingHelper
{
    public static void MergeExistingPermissions(this LocalDataContext localDataContext, File file)
    {
        if (file.Permissions == null || !file.Permissions.Any())
        {
            return;
        }

        var newPermissions = file.Permissions.Select(filePermission => 
            new { filePermission, existingPermission = localDataContext.Permission.FirstOrDefault(x => x.Id == filePermission.Id) })
            .Select(y => y.existingPermission ?? y.filePermission).ToList();

        file.Permissions = newPermissions;
    }
}