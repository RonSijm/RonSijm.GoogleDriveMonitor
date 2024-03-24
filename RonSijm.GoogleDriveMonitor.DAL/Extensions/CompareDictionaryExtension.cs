using System.Collections.Generic;
using System.Linq;

namespace RonSijm.GoogleDriveMonitor.DAL.Extensions;

// ReSharper disable once UnusedType.Global - Justification: Used by Dependency Injection
public static class CompareDictionaryExtension
{
    public static bool CompareTo(this IDictionary<string, string> dictionary1, IDictionary<string, string> dictionary2)
    {
        if (dictionary1 == null)
        {
            // Both are null, so compare dictionary2 is also null
            return dictionary2 == null;
        }

        // if dictionary2 is null and dictionary1 wasn't - they're not equal
        if (dictionary2 == null)
        {
            return false;
        }

        return dictionary1.SequenceEqual(dictionary2);
    }
}