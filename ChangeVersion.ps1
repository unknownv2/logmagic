param(
   [String]
   $Version
)

Write-Host "version is $Version"

function Update-ProjectVersions([string]$RelPath, [string]$Version, [bool]$UpdatePackageVersion)
{
   $xml = [xml](Get-Content "$PSScriptRoot\$RelPath")

   if($UpdatePackageVersion)
   {
      $xml.Project.PropertyGroup[0].VersionPrefix = $Version
   }

   foreach($other in $args)
   {
      $json.dependencies.$other = $Version

      Write-Host "set $other to $Version"
   }

   $xml.Save("$PSScriptRoot\$RelPath")
}

Update-ProjectVersions "src\LogMagic\LogMagic.csproj" $Version $true
Update-ProjectVersions "src\LogMagic.Microsoft.Azure\LogMagic.Microsoft.Azure.csproj" $Version $true
Update-ProjectVersions "src\LogMagic.Microsoft.Azure.ServiceFabric\LogMagic.Microsoft.Azure.ServiceFabric.csproj" $Version $true
Update-ProjectVersions "src\LogMagic.Microsoft.Azure.ApplicationInsights\LogMagic.Microsoft.Azure.ApplicationInsights.csproj" $Version $true