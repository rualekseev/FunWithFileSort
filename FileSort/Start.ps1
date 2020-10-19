$watch = [System.Diagnostics.Stopwatch]::StartNew()
$watch.Start()
./FileSort.exe -file=F:\Apps\result.txt -alg=insertion
$watch.Stop()
Write-Host $watch.Elapsed
