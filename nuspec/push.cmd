@echo off
rem nuget push *.nupkg -Source https://www.nuget.org/api/v2/package
nuget.exe push *.nupkg -Source https://dev.fleetcomplete.com/MFF/
pause