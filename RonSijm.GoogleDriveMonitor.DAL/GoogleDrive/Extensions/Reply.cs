using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace Google.Apis.Drive.v3.Data;

// ReSharper disable PropertyCanBeMadeInitOnly.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
// ReSharper disable once ClassNeverInstantiated.Global - Justification: Instantiated by Serialization
public partial class Reply
{
    public string CommentId { get; set; }

    [ForeignKey(nameof(CommentId))]
    public Comment Comment { get; set; }

    // File Intentionally denormalized to make it easier to query :)
    public string FileId { get; set; }
    [ForeignKey(nameof(FileId))]
    public File File { get; set; }
}