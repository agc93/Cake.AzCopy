# Settings

This Cake addin supports the majority of the options supported by the AzCopy CLI.

Note, however, that there are some options which are ignored when running on Linux due to the Linux version of AzCopy not supporting all functionality.

This addin supports both a fluent settings API and an object-oriented API.

What does this look like in practice?

A full example of the fluent API:

```csharp
AzCopy(source, destination, settings => 
    settings.UsePattern("*.txt")
        .UseDestinationAccountKey(accountKey) // or UseDestinationSignature
        .UseSourceAccountKey(accountKey) // or UseSourceSignature
        .CopyRecursively()
        .SetBlobType(BlobType.Block)
        .EnableChecksums()
        .LogToFile("./logfile") // ignored on Linux
        .AddResponseFile("./parameters.txt")
        .SetFileBehaviour(FileHandling.ExcludeNewer|FileHandling.UpdateLastModified)
        .UseDelimiter(':')
        .SetConcurrentOperationsCount(512)
        .SetContentType("text/plain")
);
```

A full example of the object API:

```csharp
var settings = new AzCopySettings {
    Pattern = "*.txt",
    DestinationKey = accountKey, // or DestinationSAS
    SourceKey = sourceKey, // or SourceSAS
    Recursive = true,
    BlobType = BlobType.Block,
    UseChecksum = true,
    LogFile = "./logfile" // ignored on Linux
    ParameterFiles = new List<string> { "./parameters.txt" },
    FileHandlingBehaviour = FileHandling.ExcludeNewer|FileHandling.UpdateLastModified,
    Delimiter = ':',
    ConcurrentOperations = 512,
    TargetContentType = "text/plain"
};
AzCopy(source, destination, settings);
```