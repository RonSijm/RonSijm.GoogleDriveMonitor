namespace RonSijm.GoogleDriveMonitor.CoreLib.Extensions;

public static class LoggingExtensions
{
    public static void LogComment(this ILogger logger, Comment comment)
    {
        var author = comment.Author != null ? comment.Author.DisplayName : UserConfig.UserDefaultValue;
        logger.LogInformation("Comment: {comment.Content} by {author}", comment.Content, author);
    }

    public static void LogFileProcessingStart(this ILogger logger, File file, File existingRecord)
    {
        if (existingRecord == null)
        {
            logger.LogInformation("Processing File: {googleId} - {name}", file.Id, file.Name);
        }
        else
        {
            logger.LogInformation("Processing existing File: {googleId}, dbId: {dbId} - {name}", file.Id, existingRecord.Id, file.Name);
        }
    }

    public static void LogCommentReply(this ILogger logger, Comment comment, Reply reply)
    {
        var replayAuthor = reply.Author != null ? comment.Author.DisplayName : UserConfig.UserDefaultValue;

        logger.LogInformation("Reply: {reply.Content} by {reply.Author.DisplayName}", reply.Content, replayAuthor);
    }
}