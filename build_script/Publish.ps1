[CmdletBinding()]
param (
	[Parameter(Mandatory=$True, position = 0)][string] $publishOutputRootDir,
	[Parameter(Mandatory=$True, position = 1)][string] $makeInstallerScriptDir,
	[Parameter(Mandatory=$True, position = 2)][string] $installAppId,
	[Parameter(Mandatory=$True, position = 3)][string] $version,
	[Parameter(Mandatory=$false)][int] $isPublishAnyCPU,
	[Parameter(Mandatory=$false)][int] $isPublishX86,
	[Parameter(Mandatory=$false)][int] $isPublishX64,
	[Parameter(Mandatory=$false)][string] $anyCPUBuildArtifactsDir,
	[Parameter(Mandatory=$false)][string] $x86BuildArtifactsDir,
	[Parameter(Mandatory=$false)][string] $x64BuildArtifactsDir,
	[Parameter(Mandatory=$false)][string] $appKeyInOa,
	[Parameter(Mandatory=$false)][string] $oaWebServiceUrl,
	[Parameter(Mandatory=$false)][string] $versionReleaseNotes,
	[Parameter(Mandatory=$false)][string] $hockeyAppApiToken,
	[Parameter(Mandatory=$false)][string] $hockeyX86AppId,
	[Parameter(Mandatory=$false)][string] $hockeyX64AppId
)

function Main() {
	
	$x86PackageFilePath = $null
	$x86PackageZipFilePath = $null
	$x86PdbZipFilePath = $null

	$x64PackageFilePath = $null
	$x64PackageZipFilePath = $null
	$x64PdbZipFilePath = $null
	
	# 如果发布 Any CPU 模式，那么就不发布 x86 和 x64 模式
	if ($isPublishAnyCPU -ne 0) {
		if (!$anyCPUBuildArtifactsDir) {
			Write-Host( "parameter 'anyCPUBuildArtifactsDir' must set when isPublishAnyCPU is open." )
			$host.SetShouldExit(-1)
			return
		}

		Write-Host( "'isPublishAnyCPU' is opened, create x86, x64 install package by any cpu mode." )

		$outputDirPath = Join-Path -Path $publishOutputRootDir -ChildPath "AnyCPU"
		CreateDir -dirPath $outputDirPath -clearDirBeforeCreate

		$args1 = @{ makeInstallerScriptDir=$makeInstallerScriptDir
					installAppId=$installAppId
					version=$version
					buildArtifactsDir=$anyCPUBuildArtifactsDir
					publishOutputDir=$outputDirPath
					packageName="setup-anycpu"
					allow64bitMode=$True
				 }
		$packageFilePath = CreateInstallPackageFile @args1
		$packageZipFilePath = CreateInstallPackageCompressFile $packageFilePath $outputDirPath
		$pdbFilePath = CreatePDBCompressFile $anyCPUBuildArtifactsDir $outputDirPath

		$x86PackageFilePath = $packageFilePath
		$x86PackageZipFilePath = $packageZipFilePath
		$x86PdbZipFilePath = $pdbFilePath

		$x64PackageFilePath = $packageFilePath
		$x64PackageZipFilePath = $packageZipFilePath
		$x64PdbZipFilePath = $pdbFilePath

	} else {
		if ($isPublishX86 -ne 0) {
			if (!$x86BuildArtifactsDir) {
				Write-Host( "parameter 'x86BuildArtifactsDir' must set when isPublishX86 is open." )
				$host.SetShouldExit(-1)
				return
			}

			Write-Host( "'isPublishX86' is opened, create x86 install package by x86 bit mode." )

			$outputDirPath = Join-Path -Path $publishOutputRootDir -ChildPath "X86"
			CreateDir -dirPath $outputDirPath -clearDirBeforeCreate

			$args1 = @{ makeInstallerScriptDir=$makeInstallerScriptDir
						installAppId=$installAppId
						version=$version
						buildArtifactsDir=$x86BuildArtifactsDir
						publishOutputDir=$outputDirPath
						packageName="setup-x86"
					 }
			$packageFilePath = CreateInstallPackageFile @args1
			$packageZipFilePath = CreateInstallPackageCompressFile $packageFilePath $outputDirPath
			$pdbFilePath = CreatePDBCompressFile $x86BuildArtifactsDir $outputDirPath

			$x86PackageFilePath = $packageFilePath
			$x86PackageZipFilePath = $packageZipFilePath
			$x86PdbZipFilePath = $pdbFilePath
		}

		if ($isPublishX64 -ne 0) {
			if (!$x64BuildArtifactsDir) {
				Write-Host( "parameter 'x64BuildArtifactsDir' must set when isPublishX64 is open." )
				$host.SetShouldExit(-1)
				return
			}

			Write-Host( "'isPublishX64' is opened, create x64 install package by x64 bit mode." )

			$outputDirPath = Join-Path -Path $publishOutputRootDir -ChildPath "X64"
			CreateDir -dirPath $outputDirPath -clearDirBeforeCreate

			$args1 = @{ makeInstallerScriptDir=$makeInstallerScriptDir
						installAppId=$installAppId
						version=$version
						buildArtifactsDir=$x64BuildArtifactsDir
						publishOutputDir=$outputDirPath
						packageName="setup-x64"
						allow64bitMode=$True
					 }
			$packageFilePath = CreateInstallPackageFile @args1
			$packageZipFilePath = CreateInstallPackageCompressFile $packageFilePath $outputDirPath
			$pdbFilePath = CreatePDBCompressFile $x64BuildArtifactsDir $outputDirPath

			$x64PackageFilePath = $packageFilePath
			$x64PackageZipFilePath = $packageZipFilePath
			$x64PdbZipFilePath = $pdbFilePath
		}
	}

	# 上传到 oa
	$upOaArgs = @{ appKeyInOa=$appKeyInOa
				   oaWebServiceUrl=$oaWebServiceUrl
				   version=$version
				   x86PackageFilePath=$x86PackageFilePath
				   x64PackageFilePath=$x64PackageFilePath
				   versionReleaseNotes=$versionReleaseNotes
				 }
	Write-Host( "Begin upload version to OA if need." )
	$uploadOa = UploadVersion2OA @upOaArgs
	if ($uploadOa -ne $True) {
		Write-Host( "Failed to upload version to OA." )
		$host.SetShouldExit(-1)
		return
	}
	
	# 上传到 hockeyapp
	if (!$hockeyAppApiToken) {
		Write-Host( "hockeyAppApiToken not set, not update version to hockeyapp." )
	} else {
		# Upload x86 version to hockeyapp
		$args1 = @{hockeyApiKey=$hockeyAppApiToken;hockeyAppId=$hockeyX86AppId;version=$version;installPackageZipPath=$x86PackageZipFilePath;pdbZipPath=$x86PdbZipFilePath}
		
		Write-Host( "Begin upload x86 version to HockeyApp if need." )
		UploadVersion2HockeyApp @args1

		
		# Upload x64 version to hockeyapp
		$args2 = @{hockeyApiKey=$hockeyAppApiToken;hockeyAppId=$hockeyX64AppId;version=$version;installPackageZipPath=$x64PackageZipFilePath;pdbZipPath=$x64PdbZipFilePath}
		
		Write-Host( "Begin upload x64 version to HockeyApp if need." )
		UploadVersion2HockeyApp @args2
	}

	$host.SetShouldExit(0)
}

function UploadVersion2OA {
	[CmdletBinding()]
	param (
		[Parameter(Mandatory=$false)][string] $appKeyInOa,
		[Parameter(Mandatory=$false)][string] $oaWebServiceUrl,
		[Parameter(Mandatory=$false)][string] $version,
		[Parameter(Mandatory=$false)][string] $x86PackageFilePath,
		[Parameter(Mandatory=$false)][string] $x64PackageFilePath,
		[Parameter(Mandatory=$false)][string] $versionReleaseNotes
	)

	if (!$appKeyInOa) {
		Write-Host( "appKeyInOa not set, not upload packages to oa." )
		return $False
	}
	if (!$oaWebServiceUrl) {
		Write-Host( "oaWebServiceUrl not set, not upload packages to oa." )
		return $False
	}
	if (!$version) {
		Write-Host( "'version' not set, can't upload version to hockeyapp." )
		return $False
	}

	$scriptDir = GetCurrentScriptDir

	# 考虑后台上传服务内存限制，包一个一个上传
	# 上传 x86 包
	$commArgs = @()
	$commArgs += ("-appKeyInOa", $appKeyInOa)
	$commArgs += ("-version", $version)
	$commArgs += ("-webServiceUrl", $oaWebServiceUrl)
	if ($versionReleaseNotes) {
		$commArgs += ("-versionReleaseNotes", $versionReleaseNotes)	
	}
	
	# 上传 x86 包
	if ($x86PackageFilePath) {
		$args1 = $commArgs
		$args1 += ("-x32PackageFile", $x86PackageFilePath)

		Write-Host( "Upload x86 version 2 OA. args:" )
		Write-Host( "$args1" )

		$uploadResult = Invoke-Expression "$scriptDir\UploadPackage2Oa.ps1 $args1"
		if ((-not $?) -or ($uploadResult -ne $True))
		{
			Write-Host "Failed to upload x86 version 2 OA."
			return $False
		}
	}

	# 上传 x64 包
	if ($x64PackageFilePath) {
		$args1 = $commArgs
		$args1 += ("-x64PackageFile", $x64PackageFilePath)

		Write-Host( "Upload x64 version 2 OA. args:" )
		Write-Host( "$args1" )

		$uploadResult = Invoke-Expression "$scriptDir\UploadPackage2Oa.ps1 $args1"
		if ((-not $?) -or ($uploadResult -ne $True))
		{
			Write-Host "Failed to upload x64 version 2 OA."
			return $False
		}
	}

	return $True
}

function UploadVersion2HockeyApp {
	[CmdletBinding()]
	param (
		[Parameter(Mandatory=$false)][string] $hockeyApiKey,
		[Parameter(Mandatory=$false)][string] $hockeyAppId,
		[Parameter(Mandatory=$false)][string] $version,
		[Parameter(Mandatory=$false)][string] $installPackageZipPath,
		[Parameter(Mandatory=$false)][string] $pdbZipPath
	)

	if (!$hockeyApiKey) {
		Write-Host( "'hockeyApiKey' not set, can't upload version to hockeyapp." )
		return $False
	}
	if (!$hockeyAppId) {
		Write-Host( "'hockeyAppId' not set, can't upload version to hockeyapp." )
		return $False
	}
	if (!$version) {
		Write-Host( "'version' not set, can't upload version to hockeyapp." )
		return $False
	}
	if (!$installPackageZipPath) {
		Write-Host( "'installPackageZipPath' not set, can't upload version to hockeyapp." )
		return $False
	}

	$args1 = @()
	$args1 += ("-apiKey", $hockeyApiKey)
	$args1 += ("-appId", $hockeyAppId)
	$args1 += ("-version", $version)
	$args1 += ("-binaryZipFile", $installPackageZipPath)
	if ($pdbZipPath) {
		$args1 += ("-dsymZipFile", $pdbZipPath)
	}
	
	Write-Host( "UploadVersion2HockeyApp. args:" )
	Write-Host( "$args1" )

	$scriptDir = GetCurrentScriptDir
	Invoke-Expression "$scriptDir\UploadZip2HockeyApp.ps1 $args1"
	return $True
}

function GetCurrentScriptDir() {
	return Split-Path $script:MyInvocation.MyCommand.Path
}

function CreateDir {
	[CmdletBinding()]
	param(
		[Parameter(Mandatory=$True, position = 0)][string] $dirPath,
		[Parameter(Mandatory=$false)][switch] $clearDirBeforeCreate
	)

	if (!$dirPath) {
		return
	}

	if ($clearDirBeforeCreate.IsPresent -and (Test-Path -Path $dirPath )) {
		Get-ChildItem $dirPath -Recurse | Remove-Item
	}

	if( -Not (Test-Path -Path $dirPath ) )
	{
		New-Item -ItemType directory -Path $dirPath
	}
}

### 
### 创建安装包文件。返回安装包文件路径
###
function CreateInstallPackageFile {
	[CmdletBinding()]
	param(
		[Parameter(Mandatory=$True, position = 0)][string] $makeInstallerScriptDir,
		[Parameter(Mandatory=$True, position = 1)][string] $installAppId,
		[Parameter(Mandatory=$True, position = 2)][string] $version,
		[parameter(Mandatory=$true, position = 3)][string] $buildArtifactsDir,
		[parameter(Mandatory=$true, position = 4)][string] $publishOutputDir,
		[parameter(Mandatory=$true, position = 5)][string] $packageName,
		[Parameter(Mandatory=$false)][switch] $allow64bitMode
	)

	$fileName = "$packageName.exe"
	$filePath = Join-Path -Path $publishOutputDir -ChildPath $fileName
	$makeInstallerScriptFilePath = Join-Path -Path $makeInstallerScriptDir -ChildPath "MakeInstaller.iss"

	$args1 = @()
	$args1 += ("-makeInstallerScriptFilePath", $makeInstallerScriptFilePath)
	$args1 += ("-MyAppId", $installAppId)
	$args1 += ("-MyAppVersion", $version)
	$args1 += ("-MyOutputDir", $publishOutputDir)
	$args1 += ("-MyOutputBaseFilename", $packageName)
	$args1 += ("-MyInstallSourceFilesDir", $buildArtifactsDir)
	if ($allow64bitMode.IsPresent) {
		$args1 += ("-Allow64bitMode", 1)
	} else {
		$args1 += ("-Allow64bitMode", 0)
	}

	$scriptDir = GetCurrentScriptDir
	Invoke-Expression "$scriptDir\MakeInstaller.ps1 $args1"

	return $filePath
}


### 
### 创建安装包文件的压缩包。返回压缩包文件路径
###
function CreateInstallPackageCompressFile {
	[CmdletBinding()]
	param(
		[parameter(Mandatory=$true, position = 0)][string] $installPackageFilePath,
		[parameter(Mandatory=$true, position = 1)][string] $publishOutputDir
	)

	$fileName = "setup-package.zip"
	$filePath = Join-Path -Path $publishOutputDir -ChildPath $fileName
	Compress-Archive -Path $installPackageFilePath -Update -DestinationPath $filePath
	return $filePath
}

### 
### 创建pdb文件的压缩包。返回压缩包文件路径
###
function CreatePDBCompressFile {
	[CmdletBinding()]
	param(
		[parameter(Mandatory=$true, position = 0)][string] $buildArtifactsDir,
		[parameter(Mandatory=$true, position = 1)][string] $publishOutputDir
	)

	$pdbFilePaths = (Get-Childitem $buildArtifactsDir -filter "*.pdb" -Recurse -Depth 5).FullName
	$fileName = "pdb.zip"
	$filePath = Join-Path -Path $publishOutputDir -ChildPath $fileName
	Compress-Archive -Path $pdbFilePaths -Update -DestinationPath $filePath

	return $filePath
}

Main