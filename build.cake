#addin Cake.Xamarin
#addin Cake.FileHelpers
//#tool NUnit.ConsoleRunner
var target = Argument("target", Argument("t", "package"));

Setup(x =>
{
    DeleteFiles("./*.nupkg");
    DeleteFiles("./output/*.*");

	if (!DirectoryExists("./output"))
		CreateDirectory("./output");
});

Task("build")
	.Does(() =>
{
	NuGetRestore("./FleetComplete.Geocoder.sln");
	DotNetBuild("./FleetComplete.Geocoder.sln", x => x
        .SetConfiguration("Release")
        .SetVerbosity(Verbosity.Minimal)
        .WithProperty("TreatWarningsAsErrors", "false")
    );
});

Task("tests")
    .Does(() =>
    {
        //NUnit3("./**/Release/FleetComplete.Geocoder.NGeoNames.Tests.dll");
    });

Task("package")
    .IsDependentOn("tests")
	.IsDependentOn("build")
	.Does(() =>
{
	NuGetPack(new FilePath("./nuspec/FleetComplete.Geocoder.nuspec"), new NuGetPackSettings());
    NuGetPack(new FilePath("./nuspec/FleetComplete.Geocoder.NGeoNames.nuspec"), new NuGetPackSettings());
	MoveFiles("./*.nupkg", "./output");
});

Task("publish")
    .IsDependentOn("package")
    .Does(() =>
{
    /*
    NuGetPush("./output/*.nupkg", new NuGetPushSettings
    {
        Source = "http://www.nuget.org/api/v2/package",
        Verbosity = NuGetVerbosity.Detailed
    });
    */
});

RunTarget(target);