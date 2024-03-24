using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable CheckNamespace - Justification: Used by Entity Framework
// ReSharper disable UnusedType.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
namespace Google.Apis.Drive.v3.Data;

public partial class ContentRestriction
{
    [Key]
    [Column("Id")]
    public int DatabaseId { get; set; }
}