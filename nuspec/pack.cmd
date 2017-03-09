@echo off
del *.nupkg
nuget pack FleetComplete.Geocoder.nuspec
nuget pack FleetComplete.Geocoder.NGeoNames.nuspec
nuget pack FleetComplete.Geocoder.CanadaGeoNameBase.nuspec
pause