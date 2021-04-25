[CmdletBinding()]
param (
	[Parameter(Mandatory=$True, position = 0)][string] $appKeyInOa,
	[Parameter(Mandatory=$True, position = 1)][string] $version,
	[Parameter(Mandatory=$True, position = 2)][string] $webServiceUrl,
	[Parameter(Mandatory=$false)][string] $x32PackageFile,
	[Parameter(Mandatory=$false)][string] $x64PackageFile,
	[Parameter(Mandatory=$false)][string] $versionReleaseNotes
)

function Main() {
	$params = @{
				appKeyInOa=$appKeyInOa
				version=$version
				webServiceUrl=$webServiceUrl
				x32PackageFile=$x32PackageFile
				x64PackageFile=$x64PackageFile
				versionReleaseNotes=$versionReleaseNotes
			 }

	$isSuccess = IntegrationAppVersionUsingDotNet @params
	if ($isSuccess -eq $true) {
		# How to return exit code from powershell to MSBuild. See below post
		# https://snagify.wordpress.com/2008/04/06/teambuild-powershell-and-exit-codes/
		$host.SetShouldExit(0)
	} else {
		$host.SetShouldExit(-1)
	}
	return $isSuccess
}


function GetFileMd5Hash{
	param(
		[Parameter(Mandatory=$true)][string] $file
	)
	$hashFromFile = Get-FileHash -Path $file -Algorithm MD5
	Write-Output $hashFromFile.Hash
}

function ParseIsSuccessIntegrationResult{
	[CmdletBinding()]
	param(
		[parameter(Mandatory=$true)][string] $resultStr
	)
	$resultObj = $resultStr | ConvertFrom-Json
	$isSuccess = $resultObj.isSuccess

	Write-Host "Integration result:$resultStr"
	if ($isSuccess -eq $true) {
		return $true
	} else {
		return $false
	}
}

# 组装服务端需要的版本信息参数值
function AssembleWebserviceParameter_VersionInfo {
	[CmdletBinding()]
	param(
		[Parameter(Mandatory=$True, position = 0)][string] $version,
		[Parameter(Mandatory=$false)][string] $x32PackageFile,
		[Parameter(Mandatory=$false)][string] $x64PackageFile,
		[Parameter(Mandatory=$false)][string] $versionReleaseNotes
	)

	# 获取版本号各位数值
	$verNums = "$version" -split "\."
	if ($verNums.Length -ne 4) {
		throw "$version is not 4 numbers seperate with dot."
	}

	# 组装发布信息
	$releaseNoteArr = $null
	if ($versionReleaseNotes) {
		$noteSeperator = "==="
		$releaseNoteArr = "$versionReleaseNotes".Trim($noteSeperator).Split($noteSeperator).Where({ $_ -ne "" })
	}
	
	# 获取文件 md5
	$x32FileMd5Hash = $null
	$x64FileMd5Hash = $null
	if ($x32PackageFile) {
		$x32FileMd5Hash = GetFileMd5Hash $x32PackageFile
	}
	if ($x64PackageFile) {
		$x64FileMd5Hash = GetFileMd5Hash $x64PackageFile
	}
	
	$newVersion = @{majorVersionNum=$verNums[0];minorVersionNum=$verNums[1];buildVersionNum=$verNums[2];reversionNum=$verNums[3]}
	if ($x32FileMd5Hash) {
		$newVersion.Add("md5X32", "$x32FileMd5Hash")
	}
	if ($x64FileMd5Hash) {
		$newVersion.Add("md5X64", "$x64FileMd5Hash")
	}

	if ($releaseNoteArr) {
		$newVersion.Add("updateNote", $releaseNoteArr)
	}

	$newVersionStr = $newVersion | ConvertTo-Json -Depth 2 
	return $newVersionStr
}

function IntegrationAppVersionUsingDotNet {
	[CmdletBinding()]
	param(
		[Parameter(Mandatory=$True, position = 0)][string] $appKeyInOa,
		[Parameter(Mandatory=$True, position = 1)][string] $version,
		[Parameter(Mandatory=$True, position = 2)][string] $webServiceUrl,
		[Parameter(Mandatory=$false)][string] $x32PackageFile,
		[Parameter(Mandatory=$false)][string] $x64PackageFile,
		[Parameter(Mandatory=$false)][string] $versionReleaseNotes
	)

	Try {
		$params = @{
				version=$version
				x32PackageFile=$x32PackageFile
				x64PackageFile=$x64PackageFile
				versionReleaseNotes=$versionReleaseNotes
			 }
		$newVersionStr = AssembleWebserviceParameter_VersionInfo @params

		Add-Type -AssemblyName 'System.Net.Http'

		$client = New-Object System.Net.Http.HttpClient
		$client.Timeout = New-TimeSpan -Days 0 -Hours 0 -Minutes 10 -Seconds 0
        $multipart  = New-Object System.Net.Http.MultipartFormDataContent
		$x32FileStream = $null
		$x64FileStream = $null

		if ($appKeyInOa) {
			$appKeyContent = New-Object System.Net.Http.StringContent($appKeyInOa)
			$multipart.Add($appKeyContent, "app_key")
		}

		if ($x32PackageFile) {
			# add x32 package file
			$x32FileStream = [System.IO.File]::OpenRead($x32PackageFile)
			$x32FileName = [System.IO.Path]::GetFileName($x32PackageFile)
			$x32FileContent = New-Object System.Net.Http.StreamContent($x32FileStream)
			$multipart.Add($x32FileContent, "file_x32", $x32FileName)
        }
        
		if ($x64PackageFile) {
			# add x64 package file
			$x64FileStream = [System.IO.File]::OpenRead($x64PackageFile)
			$x64FileName = [System.IO.Path]::GetFileName($x64PackageFile)
			$x64FileContent = New-Object System.Net.Http.StreamContent($x64FileStream)
			$multipart.Add($x64FileContent, "file_x64", $x64FileName)
		}

		if ($newVersionStr) {
			$versionContent = New-Object System.Net.Http.StringContent($newVersionStr)
			$multipart.Add($versionContent, "new_version")
		}
		
		Write-Host "Begin post version '$version'"
		Write-Host "param 'app_key': '$appKeyInOa'"
		Write-Host "param 'new_version':"
		Write-Host "$newVersionStr"

        $postTask =  $client.PostAsync($webServiceUrl, $multipart)
        $response = $postTask.Result
		$te = $postTask.Exception
        if ($te -or ($postTask.IsCompleted -eq $false)) {
            Write-Host( "FAILED to post version '$version'. upload task is canceled or faulted. may be timeout.")
            Write-Host( "execption: $te" )
            return $false
        }
        if ($response -eq $null) {
            Write-Host( "FAILED to post version '$version'. response is null." )
            return $false
        }

		$respContent = $response.Content.ReadAsStringAsync()
        if ($respContent) {
			$respResult = $respContent.Result
			Write-Host "respResult: $respResult"
			$isSuccess = ParseIsSuccessIntegrationResult $respResult
			Write-Host "Post version '$version', isSuccess:$isSuccess"
			return $isSuccess
		}
		else {
			Write-Host( "FAILED to post version '$version'" )
			return $false
		}	
	}
	Catch {
		Write-Host( "FAILED to post version '$version'" )
		Write-Host $_
		return $false
	}
	Finally {
		if ($client -ne $null) { 
			$client.CancelPendingRequests()
			$client.Dispose()
		}
		if ($multipart -ne $null) { $multipart.Dispose() }
		if ($x32FileStream -ne $null) { $x32FileStream.Dispose() }
		if ($x64FileStream -ne $null) { $x64FileStream.Dispose() }
	}
}

Main