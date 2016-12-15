msbuild src/LogMagic.sln /p:Configuration=Release
del *.nupkg
nuget pack logmagic.nuspec
nuget pack logmagic.azure.nuspec