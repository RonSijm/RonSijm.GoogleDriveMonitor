using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RonSijm.GoogleDriveMonitor.DAL.Entities;


// ReSharper disable once CheckNamespace
namespace Google.Apis.Drive.v3.Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Justification: Used by Entity Framework
// ReSharper disable PropertyCanBeMadeInitOnly.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedType.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
public partial class File 
{
    public StatusType StatusType { get; set; }

    public int ProcessId { get; set; }

    [ForeignKey(nameof(ProcessId))]
    public ProcessEntity Process { get; set; }

    [Table("FileCapabilitiesData")]
    public partial class CapabilitiesData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    public partial class ImageMediaMetadataData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }

        public partial class LocationData
        {
            [Key]
            [Column("Id")]
            public int DatabaseId { get; set; }
        }
    }

    public partial class ContentHintsData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }

        public partial class ThumbnailData
        {
            [Key]
            [Column("Id")]
            public int DatabaseId { get; set; }
        }
    }

    public partial class LabelInfoData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    public partial class ShortcutDetailsData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    public partial class VideoMediaMetadataData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    public partial class LinkShareMetadataData
    {
        [Key]
        [Column("Id")]
        public int DatabaseId { get; set; }
    }

    [InverseProperty(nameof(File))]
    public virtual ICollection<Comment> Comments { get; set; }
}