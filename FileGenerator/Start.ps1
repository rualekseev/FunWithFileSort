﻿$watch = [System.Diagnostics.Stopwatch]::StartNew()
$watch.Start()
./FileGenerator.exe -size=10485760 -alg=rand | Set-Content -Path result.txt
$watch.Stop()
Write-Host $watch.Elapsed
