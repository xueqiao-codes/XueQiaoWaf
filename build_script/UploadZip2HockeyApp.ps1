[CmdletBinding()]
param (
	[Parameter(Mandatory=$True, position = 0)][string] $apiKey,
	[Parameter(Mandatory=$True, position = 1)][string] $appId,
	[Parameter(Mandatory=$True, position = 2)][string] $version,
	[Parameter(Mandatory=$True, position = 3)][string] $binaryZipFile,
	[Parameter(Mandatory=$false)][int[]] $teams,
	[Parameter(Mandatory=$false)][int[]] $users,
	[Parameter(Mandatory=$false)][string[]] $tags,
	[Parameter(Mandatory=$false)][string] $dsymZipFile,
	[Parameter(Mandatory=$false)][string]$notes,
	[Parameter(Mandatory=$false)][string]$notesType,
	[Parameter(Mandatory=$false)][int] $notify,
	[Parameter(Mandatory=$false)][int] $status,
	[Parameter(Mandatory=$false)][int] $mandatory,
	[Parameter(Mandatory=$false)][switch] $overwrite
)

[byte[]]$crlf = 13, 10

function Main() {
	Write-Host "Begin UploadZip2HockeyApp, version[$version] of [$appId] to HockeyApp."
	Write-Host "binaryZipFile path[$binaryZipFile]."
	Write-Host "dsymZipFile path[$dsymZipFile]."

	if(!$notify){
		$notify = 1
	}
	if (!$status){
		$status = 2
	}

	$versionId = $null
	if(!$version){
		throw "you  must specify the version number if you wish to overwrite it."
	}
	$existVersion = GetHockeyAppVersion $apiKey $appId $version
	if(!$existVersion){
		# create version
		$newVersion = CreateHockeyAppVersion $apiKey $appId $version -status $status
		$versionId = $newVersion.id
	} else {
		if($overwrite.IsPresent){
			$versionId = $existVersion.id
		}
	}

	if ($versionId){
		# update binary and other text message
		# must seperate binary and dsym file update, only upload a file one time
		$updateParams = @{
			apiKey=$apiKey
			appId=$appId
			versionId=$versionId
			binaryFile=$binaryZipFile
			teams=$teams
			users=$users
			tags=$tags
			notes=$notes
			notesType=$notesType
			notify=$notify
			status=$status
			mandatory=$mandatory
		}
		UpdateHockeyAppVersion @updateParams
		
		if ($dsymZipFile){
			$updateParams = @{
				apiKey=$apiKey
				appId=$appId
				versionId=$versionId
				dsymFile=$dsymZipFile
				teams=$teams
				users=$users
				tags=$tags
				notes=$notes
				notesType=$notesType
				notify=$notify
				status=$status
				mandatory=$mandatory
			}
			UpdateHockeyAppVersion @updateParams
		}
	}

	Write-Host "End UploadZip2HockeyApp, version[$version] of [$appId] to HockeyApp."
}


#region Private Functions
function Get-AsciiBytes($str){
    return  [System.Text.Encoding]::ASCII.GetBytes($str)
}

function Write-MultiPartProperty {
    param(
        [parameter(Mandatory=$true)][System.IO.MemoryStream] $body,
        [parameter(Mandatory=$true)][string] $boundary,
        [parameter(Mandatory=$true)][string] $key,
        [string] $value
    )
    if(!$value){ return }

    $encoded = Get-AsciiBytes('--' + $boundary)
    $body.Write($encoded, 0, $encoded.Length)
    $body.Write($crlf, 0, $crlf.Length)
                
    $encoded = (Get-AsciiBytes('Content-Disposition: form-data; name="' + $key + '"'))
    $body.Write($encoded, 0, $encoded.Length)
    $body.Write($crlf, 0, $crlf.Length)
    $body.Write($crlf, 0, $crlf.Length)
                
    $encoded = (Get-AsciiBytes "$value")
    $body.Write($encoded, 0, $encoded.Length)
    $body.Write($crlf, 0, $crlf.Length)
        
}

function Write-MultiPartFile {
    param(
        [parameter(Mandatory=$true)][System.IO.MemoryStream] $body,
        [parameter(Mandatory=$true)][string] $boundary,
        [parameter(Mandatory=$true)][string] $name,
        [string] $file
    )
    if(!$file){ return }
    
    $encoded = Get-AsciiBytes('--' + $boundary)
    $body.Write($encoded, 0, $encoded.Length)
    $body.Write($crlf, 0, $crlf.Length)
                
    $fileName = (Get-ChildItem $file).Name
    $encoded = (Get-AsciiBytes('Content-Disposition: form-data; name="' + $name + '"; filename="' + $fileName + '"'))
    $body.Write($encoded, 0, $encoded.Length)
    $body.Write($crlf, 0, $crlf.Length)            

    $encoded = (Get-AsciiBytes 'Content-Type:application/octet-stream')
    $body.Write($encoded, 0, $encoded.Length)
    $body.Write($crlf, 0, $crlf.Length)
    $body.Write($crlf, 0, $crlf.Length)
                
    $encoded = [System.IO.File]::ReadAllBytes($file)
    $body.Write($encoded, 0, $encoded.Length)
}

function Close-MultiPartStream{
    param(
        [System.IO.MemoryStream] $body,
        [string] $boundary
    )
    
    $encoded = Get-AsciiBytes('--' + $boundary)
    $body.Write($crlf, 0, $crlf.Length)
    $body.Write($encoded, 0, $encoded.Length)
                
    $encoded = (Get-AsciiBytes '--');
    $body.Write($encoded, 0, $encoded.Length);
    $body.Write($crlf, 0, $crlf.Length);
}
#endregion

<#
    .SYNOPSIS 
        Lists all versions of a specific app
    .DESCRIPTION
        In order to use the `-overwrite` parameter on `Push-ToHockeyApp`, we must be able to figure out if the version already exists. 
        This method serves to help us find if it exists.
    .PARAMETER apiKey
        HockeyApp API Key
    .PARAMETER appId
        The ID of the app to get versions of
    .PARAMETER page
        The page number to show (only useful if there are > 500 versions
#>
function GetHockeyAppVersions{
    [CmdletBinding()]
	param(
        [parameter(Mandatory=$true, position = 0)][string] $apiKey,
        [parameter(Mandatory=$true, position = 1)][string] $appId,
        [parameter(Mandatory=$false)][int] $page
    )
    [System.Uri] $url = "https://rink.hockeyapp.net/api/2/apps/$appId/app_versions"
    $headers = @{"X-HockeyAppToken"="$apiKey"}

    $body = New-Object System.IO.MemoryStream
    $boundary = [Guid]::NewGuid().ToString().Replace('-','')

    if($page){
        Write-MultiPartProperty $body $boundary 'page' $page
        Close-MultiPartStream $body $boundary
    }
    
    try {
        (New-Object System.Net.WebClient).Proxy.Credentials = [System.Net.CredentialCache]::DefaultNetworkCredentials 
        if($page){
            $response = Invoke-RestMethod -Headers $headers -Uri $URL -Method 'POST' -ContentType "multipart/form-data; boundary=$boundary" -Body $body.ToArray()
        } else {
            $response = Invoke-RestMethod -Headers $headers -Uri $URL
        }
        Write-Output $response
    }
    catch [System.Net.WebException] {
        Write-Host( "FAILED to reach '$URL': $_" )
        throw $_
    }
}

<#
    .SYNOPSIS 
        Gets a single version of an app if it exits
    .DESCRIPTION
        In order to use the `-overwrite` parameter on `Push-ToHockeyApp`, we must be able to figure out if the version already exists. 
        This method serves to help us find if it exists.
    .PARAMETER apiKey
        HockeyApp API Key
    .PARAMETER appId
        The ID of the app to get version of
    .PARAMETER appVersion
        The version of the app to get the details of
#>
function GetHockeyAppVersion{
    [CmdletBinding()]
	param(
        [parameter(Mandatory=$true, position = 0)][string] $apiKey,
        [parameter(Mandatory=$true, position = 1)][string] $appId,
        [parameter(Mandatory=$true, position = 2)][string] $appVersion
    )
    $allVersions = GetHockeyAppVersions $apiKey $appId
    $resp = $allVersions.app_versions | ? { $_.version -eq $appVersion  }
    Write-Output $resp
}

function CreateHockeyAppVersion {
	[CmdletBinding()]
	param(
        [parameter(Mandatory=$true, position = 0)][string] $apiKey,
        [parameter(Mandatory=$true, position = 1)][string] $appId,
        [parameter(Mandatory=$true, position = 2)][string] $appVersion,
		[parameter(Mandatory=$false)][int[]] $teams,
        [parameter(Mandatory=$false)][int[]] $users,
        [parameter(Mandatory=$false)][string[]] $tags,
        [parameter(Mandatory=$false)][string]$notes,
        [parameter(Mandatory=$false)][string]$notesType,
        [parameter(Mandatory=$false)][int] $status
	)
	
	[System.Uri] $url = "https://rink.hockeyapp.net/api/2/apps/$appId/app_versions/new"
	$method = 'POST'
	$headers = @{"X-HockeyAppToken"="$apiKey"}

    $body = New-Object System.IO.MemoryStream
    $boundary = [Guid]::NewGuid().ToString().Replace('-','')
	
	Write-Host "Create version [$version] of [$appId] to HockeyApp."
	
	if ($appVersion){
		Write-MultiPartProperty $body $boundary 'bundle_version' $appVersion 
	}
	if ($teams){
		Write-MultiPartProperty $body $boundary 'teams' $($teams -join ',')
	}
	if ($users){
	    Write-MultiPartProperty $body $boundary 'users' $($users -join ',') 
	}
	if ($tags){
		Write-MultiPartProperty $body $boundary 'tags' $($tags -join ',') 
	}
	if ($status){
		Write-MultiPartProperty $body $boundary 'status' $status 
	}
	if ($notes){
		Write-MultiPartProperty $body $boundary 'notes' $notes
	}
	if ($notesType){
		Write-MultiPartProperty $body $boundary 'notes_type' $notesType 
	}
    Close-MultiPartStream $body $boundary

    try {
        (New-Object System.Net.WebClient).Proxy.Credentials = [System.Net.CredentialCache]::DefaultNetworkCredentials 
        $response = Invoke-RestMethod -Headers $headers -Uri $URL -Method $method -ContentType "multipart/form-data; boundary=$boundary" -Body $body.ToArray()
        Write-Output $response
    }
    catch [System.Net.WebException] {
        Write-Host( "FAILED to reach '$URL': $_" )
        throw $_
    }
}

function UpdateHockeyAppVersion {
	[CmdletBinding()]
	param(
		[parameter(Mandatory=$true, position = 0)][string] $apiKey,
        [parameter(Mandatory=$true, position = 1)][string] $appId,
		[parameter(Mandatory=$true, position = 2)][string] $versionId,
		[parameter(Mandatory=$false)][string] $binaryFile,
		[parameter(Mandatory=$false)][int[]] $teams,
        [parameter(Mandatory=$false)][int[]] $users,
        [parameter(Mandatory=$false)][string[]] $tags,
        [parameter(Mandatory=$false)][string] $dsymFile,
        [parameter(Mandatory=$false)][string]$notes,
        [parameter(Mandatory=$false)][string]$notesType,
        [parameter(Mandatory=$false)][int] $notify,
        [parameter(Mandatory=$false)][int] $status,
        [parameter(Mandatory=$false)][int] $mandatory
	)
	
	[System.URI] $url = "https://rink.hockeyapp.net/api/2/apps/$appId/app_versions/$versionId"
	$method = 'PUT'
	$headers = @{"X-HockeyAppToken"="$apiKey"}
	
	$body = New-Object System.IO.MemoryStream
    $boundary = [Guid]::NewGuid().ToString().Replace('-','')
	
	Write-Host "Update version id [$versionId] of [$appId] to HockeyApp."
	
	if ($teams){
		Write-MultiPartProperty $body $boundary 'teams' $($teams -join ',')
	}
	if ($users){
	    Write-MultiPartProperty $body $boundary 'users' $($users -join ',') 
	}
	if ($tags){
		Write-MultiPartProperty $body $boundary 'tags' $($tags -join ',') 
	}
	if ($status){
		Write-MultiPartProperty $body $boundary 'status' $status 
	}
	if ($notify){
		Write-MultiPartProperty $body $boundary 'notify' $notify 
	}
	if($mandatory){
		Write-MultiPartProperty $body $boundary 'mandatory' $mandatory 
	}
	if ($notes){
		Write-MultiPartProperty $body $boundary 'notes' $notes
	}
	if ($notesType){
		Write-MultiPartProperty $body $boundary 'notes_type' $notesType 
	}
	if ($dsymFile){
		Write-MultiPartFile $body $boundary 'dsym' $dsymFile
	}
	if ($binaryFile){
		Write-MultiPartFile $body $boundary 'ipa' $binaryFile
	}
    Close-MultiPartStream $body $boundary
	
	try {
        (New-Object System.Net.WebClient).Proxy.Credentials = [System.Net.CredentialCache]::DefaultNetworkCredentials 
        $response = Invoke-RestMethod -Headers $headers -Uri $URL -Method $method -ContentType "multipart/form-data; boundary=$boundary" -Body $body.ToArray()
        Write-Output $response
    }
    catch [System.Net.WebException] {
        Write-Host( "FAILED to reach '$URL': $_" )
        throw $_
    }
}

Main