# Getting Started

## Install AzCopy

First, you'll need to make sure you have AzCopy installed. You can find the full documentation of how to install AzCopy for Windows [here](https://docs.microsoft.com/en-us/azure/storage/storage-use-azcopy#download-and-install-azcopy) and AzCopy for Linux [here](https://docs.microsoft.com/en-us/azure/storage/storage-use-azcopy-linux#download-and-install-azcopy).

Since this addin relies on the underlying AzCopy CLI to perform copy operations, it runs on either Linux or Windows

> [!WARNING]
> Since AzCopy is not supported on macOS, this addin is also not supported for macOS.

> [!INFO]
> For this addin to work, you need to make sure one of `AzCopy.exe` or `azcopy` is available in the PATH or the `tools/` folder.

## Including the addin

At the top of your script, just add the following to install the addin:

```
#addin nuget:?package=Cake.AzCopy
```

## Usage

The addin exposes a single (overloaded) method alias `AzCopy` to use when for copy operations.

To use the defaults, just run the `AzCopy(string, string)` alias:

```csharp
AzCopy("https://myaccount.blob.core.windows.net/mycontainer/", "~/temp/");
```

To use the fluent settings API, just run the `AzCopy(string, string, Action<AzCopySettings>)` alias:

```csharp
AzCopy("https://myaccount.blob.core.windows.net/mycontainer/", "~/temp/", settings => settings.UsePattern("*.txt"));
```

To use the object settings API, just run the `AzCopy(string, string, AzCopySettings)` alias:

```csharp
var settings = new AzCopySettings { Pattern = "*.txt" };
AzCopy("https://myaccount.blob.core.windows.net/mycontainer/", "~/temp/", settings);
```

The addin will take care of adapting the commands and options to the platform your build is running on.

## Settings

Full information on the various settings available is provided in the [Settings documentation](settings.md).