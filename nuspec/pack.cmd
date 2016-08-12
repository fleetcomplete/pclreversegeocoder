@echo off
del *.nupkg
nuget pack FleetComplete.Geocoder.nuspec
nuget pack FleetComplete.Geocoder.NGeoNames.nuspec
pause