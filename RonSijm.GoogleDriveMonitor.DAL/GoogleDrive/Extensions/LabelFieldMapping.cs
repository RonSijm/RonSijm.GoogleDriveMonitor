using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace Google.Apis.Drive.v3.Data;

// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedAutoPropertyAccessor.Global - Justification: Used by Entity Framework
public class LabelFieldMapping
{
    [Key]
    [Column("Id")]
    public int DatabaseId { get; set; }

    public string Name { get; set; }

    public LabelField LabelField { get; set; }
}