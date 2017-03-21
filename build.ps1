param(
   [switch]
   $Publish,

   [string]
   $NuGetApiKey
)

$VersionPrefix = "2"
$VersionSuffix = "2.6.0"

$SlnPath = "logmagic.sln"
$AssemblyVersion = "$VersionPrefix.0.0.0"
$PackageVersion = "$VersionPrefix.$VersionSuffix"
Write-Host "version: $PackageVersion, assembly version: $AssemblyVersion"

function Set-VstsBuildNumber($BuildNumber)
{
   Write-Verbose -Verbose "##vso[build.updatebuildnumber]$BuildNumber"
}

function Update-ProjectVersion([string]$Path)
{
   $xml = [xml](Get-Content $Path)

   if($xml.Project.PropertyGroup.Count -eq $null)
   {
      $pg = $xml.Project.PropertyGroup
   }
   else
   {
      $pg = $xml.Project.PropertyGroup[0]
   }

   $pg.Version = $PackageVersion
   $pg.FileVersion = $PackageVersion
   $pg.AssemblyVersion = $AssemblyVersion

   $xml.Save($Path)
}

function Exec($Command)
{
   Invoke-Expression $Command
   if($LASTEXITCODE -ne 0)
   {
      Write-Error "command failed (error code: $LASTEXITCODE)"
      exit 1
   }
}

# General validation
if($Publish -and (-not $NuGetApiKey))
{
   Write-Error "Please specify nuget key to publish"
   exit 1
}

# Update versioning information
Get-ChildItem *.csproj -Recurse | Where-Object {-not($_.Name -like "*test*") -and -not($_.Name -like "*console*")} | % {
   $path = $_.FullName
   Write-Host "setting version on $path"
   Update-ProjectVersion $path
}
Set-VstsBuildNumber $PackageVersion

# Restore packages
Exec "dotnet restore $SlnPath"

# Build solution
Get-ChildItem *.nupkg -Recurse | Remove-Item -Verbose
Exec "dotnet build $SlnPath -c release"

# Run the tests
Exec "dotnet test test\LogMagic.Test\LogMagic.Test.csproj"

# publish the nugets
if($Publish.IsPresent)
{
   Write-Host "publishing nugets..."

   Get-ChildItem *.nupkg -Recurse | Where-Object {$_.FullName -like "*release*" } | % {
      $path = $_.FullName
      Write-Host "publishing from $path"

      Exec "nuget push $path -Source https://www.nuget.org/api/v2/package -ApiKey $NuGetApiKey"
   }
}

Write-Host "build succeeded."