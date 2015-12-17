msbuild src/LogMagic.sln /p:Configuration=Release
nuget pack logmagic.nuspec
nuget pack logmagic.azure.nuspec