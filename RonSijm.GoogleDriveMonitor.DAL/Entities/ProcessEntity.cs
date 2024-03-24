using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Google.Apis.Drive.v3.Data;

namespace RonSijm.GoogleDriveMonitor.DAL.Entities;

[Table(TableName)]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Justification: Used by Entity Framework
// ReSharper disable PropertyCanBeMadeInitOnly.Global - Justification: Used by Entity Framework
public class ProcessEntity
{
    public const string TableName = "Process";

    [Key]
    public int Id { get; set; }

    public StatusType Status { get; set; }

    public DateTime Started { get; set; }
    public DateTime? Finished { get; set; }

    [InverseProperty(TableName)]
    public virtual ICollection<File> Files { get; set; }
}