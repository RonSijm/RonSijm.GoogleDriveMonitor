using RonSijm.GoogleDriveMonitor.CoreLib.Extensions;

namespace RonSijm.GoogleDriveMonitor.CoreLib.Processors;

// ReSharper disable once ClassNeverInstantiated.Global - Justification: Used by DI
public class GoogleDriveFileCommentProcessor(ILogger<GoogleDriveFileCommentProcessor> logger, LocalDataContext localDataContext)
{
    public async Task FetchAndProcessComments(File file, DriveService service, CancellationToken cancellationToken)
    {
        CommentList commentsResponse;

        try
        {
            commentsResponse = await service.Comments.List(file.Id)
                       .With(x => x.IncludeDeleted = true)
                       .With(x => x.Fields = "*").ExecuteAsync(cancellationToken);

        }
        catch (Exception)
        {
            return;
        }

        if (commentsResponse.Comments is { Count: > 0 })
        {
            foreach (var comment in commentsResponse.Comments)
            {
                logger.LogComment(comment);

                var isExistingComment = localDataContext.Comments.FirstOrDefault(x => x.Id == comment.Id);

                // TODO: Maybe do a diff between existing and new comment, to see if it's been modified.
                if (isExistingComment == null)
                {
                    comment.FileId = file.Id;

                    foreach (var reply in comment.Replies)
                    {
                        logger.LogCommentReply(comment, reply);
                    }

                    UserManager.FixUsers(comment);
                    localDataContext.Add(comment);
                    await localDataContext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}