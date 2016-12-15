param($installPath, $toolsPath, $package, $project)

$snippetFolder = "LogMagic"
$source = "$toolsPath\vssnippets\*"
$vsVersion = [System.Version]::Parse($dte.Version)
$versionDir = "20" + ($vsVersion.Major + 1).ToString()
$documentsDir = [environment]::getfolderpath("mydocuments")
$myCodeSnippetsFolder = "$documentsDir\Visual Studio $versionDir\Code Snippets\Visual C#\My Code Snippets\"


write-host ========================================================================================================================================================================

if (!(Test-Path $myCodeSnippetsFolder))
{
	write-host Creating snippets folder $myCodeSnippetsFolder
	write-host 

	New-Item $myCodeSnippetsFolder -itemType "directory"
}

$destination = "$myCodeSnippetsFolder$snippetFolder"
if (!(Test-Path $destination))
{
	New-Item $destination -itemType "directory"
}

write-host Copying snippets to $destination
write-host 

Copy-Item $source -Destination $destination -Recurse -Force

write-host Snippets are available for every project in every solution!
write-host Browse https://github.com/aloneguid/logmagic to see which snippets are available.
write-host
write-host To uninstall snippets just remove $destination directory
write-host ========================================================================================================================================================================