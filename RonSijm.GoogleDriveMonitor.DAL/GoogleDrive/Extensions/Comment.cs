using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace Google.Apis.Drive.v3.Data;

// ReSharper disable PropertyCanBeMadeInitOnly.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedType.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
public partial class Comment
{
    public partial class QuotedFileContentData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    public string FileId { get; set; }

    [ForeignKey(nameof(FileId))]
    public File File { get; set; }
}