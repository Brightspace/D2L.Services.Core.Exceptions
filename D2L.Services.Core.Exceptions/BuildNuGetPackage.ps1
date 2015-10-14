do {
	# Cleanup old packages
	rm *.nupkg 2> $nul
	rm publish-*.bat 2> $nul

	# Get version number form AssemblyInfo.cs
	$assemblyInfo = Get-Content .\Properties\AssemblyInfo.cs

	$regex = [Regex]"AssemblyVersion\(\s*""(\d+\.\d+\.\d+).*""\s*\)"
	$match = $regex.Match( $assemblyInfo )

	if( !$match.Success ) {
		Write-Host
		Write-Host "Could not find version number in AssemblyInfo.cs" -ForegroundColor "red"
		break
	}

	$version = $match.Groups[1].Value

	# Download NuGet if it does not already exist
	$nugetLocation = (Get-Item .).FullName + "\NuGet.exe"

	if( !( Test-Path $nugetLocation ) ) {
		(New-Object System.Net.WebClient).DownloadFile( "https://www.nuget.org/nuget.exe", $nugetLocation )
	}

	# Make NuGet package
	.\NuGet pack .\D2L.Services.Core.Exceptions.csproj -Version $version -Properties "Configuration=Release" -Build -NonInteractive

	if( !$? ) {
		Write-Host
		Write-Host "Build failed" -ForegroundColor "red"
		break
	}

	# Make the publish .bat file
	$pushScript = ".\NuGet push D2L.Services.Core.Exceptions." + $version + ".nupkg REDACTED -s http://nuget.build.d2l/nuget/stable & pause"
	[System.IO.File]::WriteAllLines( ".\publish-" + $version + ".bat", $pushScript, [System.Text.Encoding]::ASCII)

	# Wait for user acknowledgement
	Write-Host
	Write-Host ("Package built successfully. Run publish-" + $version + ".bat to publish your changes.") -ForegroundColor "green"
} while( $false )

cmd /C "pause"