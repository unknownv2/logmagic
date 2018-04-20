$BuildNo = $env:APPVEYOR_BUILD_NUMBER
$Major = 2
$Minor = 8
$Patch = 1
$IsPrerelease = $false

if($BuildNo -eq $null)
{
   $BuildNo = "1"
}

$vt = @{
   "LogMagic.Microsoft.Azure.ServiceFabric.v2.csproj" = (5, 6, $BuildNo);
   "LogMagic.Microsoft.Azure.ServiceFabric.v3.csproj" = (6, 1, $BuildNo);
}

$Copyright = "Copyright (c) 2015-2018 by Ivan Gavryliuk"
$PackageIconUrl = "http://i.isolineltd.com/nuget/config.net.png"
$PackageProjectUrl = "https://github.com/aloneguid/config"
$RepositoryUrl = "https://github.com/aloneguid/config"
$Authors = "Ivan Gavryliuk (@aloneguid)"
$PackageLicenseUrl = "https://github.com/aloneguid/config/blob/master/LICENSE"
$RepositoryType = "GitHub"

$SlnPath = "LogMagic.sln"

function Update-ProjectVersion($File)
{
   Write-Host "updating $File ..."

   $over = $vt.($File.Name)
   if($over -eq $null) {
      $thisMajor = $Major
      $thisMinor = $Minor
      $thisPatch = $Patch
   } else {
      $thisMajor = $over[0]
      $thisMinor = $over[1]
      $thisPatch = $over[2]
   }

   $xml = [xml](Get-Content $File.FullName)

   if($xml.Project.PropertyGroup.Count -eq $null)
   {
      $pg = $xml.Project.PropertyGroup
   }
   else
   {
      $pg = $xml.Project.PropertyGroup[0]
   }

   if($IsPrerelease) {
      $suffix = "-ci-" + $BuildNo.PadLeft(5, '0')
   } else {
      $suffix = ""
   }

   
   [string] $fv = "{0}.{1}.{2}.{3}" -f $thisMajor, $thisMinor, $thisPatch, $BuildNo
   [string] $av = "{0}.0.0.0" -f $thisMajor
   [string] $pv = "{0}.{1}.{2}{3}" -f $thisMajor, $thisMinor, $thisPatch, $suffix

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

# Update versioning information
Get-ChildItem *.csproj -Recurse | Where-Object {-not($_.Name -like "*test*") -and -not($_.Name -like "*console*") -and -not($_.Name -like "*FabricApp*") -and -not($_.Name -like "*Simulator*") -and -not ($_.Name -like "*WebApi*")} | % {
   Write-Host "setting version on $($_.FullName)"
   Update-ProjectVersion $_
}

# Restore packages
Exec "dotnet restore $SlnPath"