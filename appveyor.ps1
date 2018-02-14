param(
   [switch]
   $Publish,

   [string]
   $NuGetApiKey
)

$gv = $env:APPVEYOR_BUILD_VERSION
$bn = $env:APPVEYOR_BUILD_NUMBER
if($gv -eq $null)
{
   $gv = "2.5.0"
}

$vt = @{
   "LogMagic.Storage.Net.csproj" = "1.0.0-alpha-$bn";
   "LogMagic.Microsoft.Azure.ServiceFabric.v2.csproj" = "5.6.204.$bn";
   "LogMagic.Microsoft.Azure.ServiceFabric.v3.csproj" = "6.1.456.$bn";
}

$Copyright = "Copyright (c) 2015-2017 by Ivan Gavryliuk"
$PackageIconUrl = "http://i.isolineltd.com/nuget/logmagic.png"
$PackageProjectUrl = "https://github.com/aloneguid/logmagic"
$RepositoryUrl = "https://github.com/aloneguid/logmagic"
$Authors = "Ivan Gavryliuk (@aloneguid)"
$PackageLicenseUrl = "https://github.com/aloneguid/logmagic/blob/master/LICENSE"
$RepositoryType = "GitHub"
$SlnPath = "logmagic.sln"

function Update-ProjectVersion($File)
{
   $v = $vt.($File.Name)
   if($v -eq $null) { $v = $gv }

   $xml = [xml](Get-Content $File.FullName)

   if($xml.Project.PropertyGroup.Count -eq $null)
   {
      $pg = $xml.Project.PropertyGroup
   }
   else
   {
      $pg = $xml.Project.PropertyGroup[0]
   }

   $parts = $v -split "\."
   $bv = $parts[2]
   if($bv.Contains("-")) { $bv = $bv.Substring(0, $bv.IndexOf("-"))}
   $fv = "{0}.{1}.{2}.0" -f $parts[0], $parts[1], $bv
   $av = "{0}.0.0.0" -f $parts[0]
   $pv = $v

   $pg.Version = $pv
   $pg.FileVersion = $fv
   $pg.AssemblyVersion = $av

   Write-Host "$($File.Name) => fv: $fv, av: $av, pkg: $pv"

   $pg.Copyright = $Copyright
   $pg.PackageIconUrl = $PackageIconUrl
   $pg.PackageProjectUrl = $PackageProjectUrl
   $pg.RepositoryUrl = $RepositoryUrl
   $pg.Authors = $Authors
   $pg.PackageLicenseUrl = $PackageLicenseUrl
   $pg.RepositoryType = $RepositoryType

   $xml.Save($File.FullName)
}

function Exec($Command, [switch]$ContinueOnError)
{
   Invoke-Expression $Command
   if($LASTEXITCODE -ne 0)
   {
      Write-Error "command failed (error code: $LASTEXITCODE)"

      if(-not $ContinueOnError.IsPresent)
      {
         exit 1
      }
   }
}

# Restore packages
Exec "dotnet restore $SlnPath"

# Update versioning information
Get-ChildItem *.csproj -Recurse | Where-Object {-not($_.Name -like "*test*") -and -not($_.Name -like "*console*") -and -not($_.Name -like "*FabricApp*") -and -not($_.Name -like "*Simulator*")} | % {
   Write-Host "setting version on $($_.FullName)"
   Update-ProjectVersion $_
}