$watch = [System.Diagnostics.Stopwatch]::StartNew()
$watch.Start()
./FileGenerator.exe -size=10737418240 -alg=fast | Set-Content -Path result.txt
$watch.Stop()
Write-Host $watch.Elapsed
