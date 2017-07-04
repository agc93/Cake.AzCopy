#tool "nuget:?package=GitVersion.CommandLine"
#load "helpers.cake"
#tool nuget:?package=docfx.console
#addin nuget:?package=Cake.DocFx

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildFrameworks = Argument("frameworks", "netstandard1.6;net45");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/Cake.AzCopy.sln");
var projects = GetProjects(solutionPath, configuration);
var artifacts = "./dist/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));
GitVersion versionInfo = null;
var frameworks = buildFrameworks.Split(new[] { ";", ","}, StringSplitOptions.RemoveEmptyEntries);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	versionInfo = GitVersion();
	Information("Building for version {0}", versionInfo.FullSemVer);
	Verbose("Building for " + string.Join(", ", frameworks));
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	// Clean solution directories.
	foreach(var path in projects.AllProjectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	//NuGetRestore(solutionPath);
	foreach (var project in projects.AllProjectPaths) {
		DotNetCoreRestore(project.FullPath);
	}
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	foreach(var framework in frameworks) {
		foreach (var project in projects.SourceProjectPaths) {
			var settings = new DotNetCoreBuildSettings {
				Framework = framework,
				Configuration = configuration,
				NoIncremental = true,
			};
			DotNetCoreBuild(project.FullPath, settings);
		}
	}
	
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
    CreateDirectory(testResultsPath);
	if (projects.TestProjects.Any()) {

		var settings = new DotNetCoreTestSettings {
			Configuration = configuration
		};

		foreach(var project in projects.TestProjects) {
			DotNetCoreTest(project.Path.FullPath, settings);
		}
	}
});

Task("Generate-Docs").Does(() => {
	DocFxMetadata("./docfx/docfx.json");
	DocFxBuild("./docfx/docfx.json");
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
});

Task("Post-Build")
	.IsDependentOn("Build")
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Generate-Docs")
	.Does(() =>
{
	CreateDirectory(artifacts + "build");
	CreateDirectory(artifacts + "modules");
	foreach (var project in projects.SourceProjects) {
		CreateDirectory(artifacts + "build/" + project.Name);
		foreach (var framework in frameworks) {
			var frameworkDir = artifacts + "build/" + project.Name + "/" + framework;
			CreateDirectory(frameworkDir);
			var files = GetFiles(project.Path.GetDirectory() + "/bin/" + configuration + "/" + framework + "/" + project.Name +".*");
			CopyFiles(files, frameworkDir);
		}
	}
});

Task("NuGet")
	.IsDependentOn("Post-Build")
	.Does(() => 
{
	CreateDirectory(artifacts + "package");
	Information("Building NuGet package");
	var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == versionInfo.MajorMinorPatch);
	var content = GetContent(frameworks, projects, configuration);
	var settings = new NuGetPackSettings {
		Id				= "Cake.AzCopy",
		Version			= versionInfo.NuGetVersionV2,
		Title			= "Cake.AzCopy",
		Authors		 	= new[] { "Alistair Chapman" },
		Owners			= new[] { "achapman", "cake-contrib" },
		Description		= "A simple Cake addin powered by AzCopy for uploading and downloading to/from Azure Storage (including Blob, Table and Files)",
		ReleaseNotes	= versionNotes != null ? versionNotes.Notes.ToList() : new List<string>(),
		Summary			= "A simple Cake addin for AzCopy.",
		ProjectUrl		= new Uri("https://github.com/agc93/Cake.AzCopy"),
		IconUrl			= new Uri("https://cdn.rawgit.com/cake-contrib/graphics/a5cf0f881c390650144b2243ae551d5b9f836196/png/cake-contrib-medium.png"),
		LicenseUrl		= new Uri("https://raw.githubusercontent.com/agc93/Cake.AzCopy/master/LICENSE"),
		Copyright		= "Alistair Chapman 2017",
		Tags			= new[] { "cake", "build", "script", "azure", "azcopy" },
		OutputDirectory = artifacts + "/package",
		Files			= content,
		//KeepTemporaryNuSpecFile = true
	};

	NuGetPack(settings);
});

Task("Default")
	.IsDependentOn("NuGet");

RunTarget(target);
