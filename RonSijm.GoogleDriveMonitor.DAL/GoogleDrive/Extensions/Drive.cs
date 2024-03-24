using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace - Justification: Used by Entity Framework
// ReSharper disable UnusedType.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
namespace Google.Apis.Drive.v3.Data;

public partial class Drive
{
    [Table("DriveCapabilitiesData")]
    public partial class CapabilitiesData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }
}