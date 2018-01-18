Task("NuGet")
	.IsDependentOn("Post-Build")
	.Does(() => 
{
	CreateDirectory(artifacts + "package");
	Information("Building NuGet package");
	var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == packageVersion);
	var content = GetContent(frameworks, projects, configuration);
	var settings = new NuGetPackSettings {
		Id				= "Cake.AzCopy",
		Version			= packageVersion,
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