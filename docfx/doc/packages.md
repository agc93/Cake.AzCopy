# Packages

You can include the addin in your script with:

```csharp
#addin nuget:?package=Cake.AzCopy

//or to use the latest development release
#addin nuget:?package=Cake.AzCopy&prerelease
```

The NuGet prerelease packages are automatically built and deployed from the `develop` branch so they can be considered bleeding-edge while the non-prerelease packages will be much more stable.

Versioning is predominantly SemVer-compliant so you can set your version constraints if you're worried about changes.

> [!NOTE]
> These packages **do not** include AzCopy itself so you will need to have that available in your environment first.