using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Google.Apis.Drive.v3.Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedMember.Global - Justification: Used by Entity Framework
// ReSharper disable UnusedType.Global - Justification: Used by Entity Framework
public partial class Label
{
    public virtual List<LabelFieldMapping> LabelFields => Fields.Select(x => new LabelFieldMapping { Name = x.Key, LabelField = x.Value }).ToList();
}