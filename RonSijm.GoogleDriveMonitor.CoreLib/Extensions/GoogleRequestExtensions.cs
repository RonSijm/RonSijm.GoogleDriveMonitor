namespace RonSijm.GoogleDriveMonitor.CoreLib.Extensions;

/// <summary>
/// Google as a pretty annoying syntax of needing to create a request,
/// Then Setting the correct fields,
/// Then Executing the request. This class fixes that.
/// </summary>
public static class GoogleRequestExtensions
{
    /// <summary>
    /// Sets specified field or fields.
    /// Can be used to set everything,
    /// Or can be used fluently to set properties per field,
    /// Whatever style you prefer.
    /// </summary>
    public static T With<T>(this T request, Action<T> settings)
    {
        settings(request);
        return request;
    }
}