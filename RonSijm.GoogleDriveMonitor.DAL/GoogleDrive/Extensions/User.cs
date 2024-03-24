using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RonSijm.GoogleDriveMonitor.DAL;

// ReSharper disable once CheckNamespace
namespace Google.Apis.Drive.v3.Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Justification: Used by Entity Framework
// ReSharper disable PropertyCanBeMadeInitOnly.Global - Justification: Used by Entity Framework
public partial class User
{
    [Key]
    [Column("Id")]
    public int DatabaseId { get; set; }

    public virtual ICollection<File> Files { get; set; }

    public string GetIdentifier()
    {
        if (!string.IsNullOrWhiteSpace(EmailAddress))
        {
            return EmailAddress;
        }

        if (!string.IsNullOrWhiteSpace(PhotoLink))
        {
            return PhotoLink;
        }

        if (!string.IsNullOrWhiteSpace(DisplayName))
        {
            return DisplayName;
        }

        return UserConfig.UserDefaultValue;
    }
}