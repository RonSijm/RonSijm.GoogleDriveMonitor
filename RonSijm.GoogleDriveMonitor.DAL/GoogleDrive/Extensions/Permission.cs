using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
// ReSharper disable UnusedType.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
namespace Google.Apis.Drive.v3.Data;

public partial class Permission
{
    public partial class PermissionDetailsData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    public partial class TeamDrivePermissionDetailsData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }
}