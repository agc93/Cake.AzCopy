using System;

namespace Cake.AzCopy
{
    [Flags]
    public enum FileHandling
    {
        UpdateLastModified = 0,
        ExcludeNewerSource = 1,
        ExcludeOlderSource = 2,
        ///<remarks>This value has no effect on Linux</remarks>
        ArchiveOnly = 4
    }
}