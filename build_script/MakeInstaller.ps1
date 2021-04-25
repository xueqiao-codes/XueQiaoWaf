[CmdletBinding()]
param (
	[Parameter(Mandatory=$True, position = 0)][string] $makeInstallerScriptFilePath,
	[Parameter(Mandatory=$True, position = 1)][string] $MyAppId,
	[Parameter(Mandatory=$True, position = 2)][string] $MyAppVersion,
	[Parameter(Mandatory=$True, position = 3)][string] $MyOutputDir,
	[Parameter(Mandatory=$True, position = 4)][string] $MyOutputBaseFilename,
	[Parameter(Mandatory=$True, position = 5)][string] $MyInstallSourceFilesDir,
	[Parameter(Mandatory=$True, position = 6)][int] $Allow64bitMode
)
Write-Host "Begin Creating setup"
$ArchitecturesInstallIn64BitMode = ""
if ($Allow64bitMode -ne 0) {
	$ArchitecturesInstallIn64BitMode = "x64"
}
& iscc.exe $makeInstallerScriptFilePath /O+ /Q /dMyAppId=$MyAppId /dMyAppVersion=$MyAppVersion /dMyOutputDir=$MyOutputDir /dMyOutputBaseFilename=$MyOutputBaseFilename /dMyInstallSourceFilesDir=$MyInstallSourceFilesDir /dArchitecturesInstallIn64BitMode=$ArchitecturesInstallIn64BitMode
if (-not $?)
{
	Write-Host "Creating setup failed"
} else 
{
	Write-Host "Success Creating setup"
}
