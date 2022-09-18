param(
    $unityEditorPath = "C:\Users\amiotj\Desktop\UnityEditors\2020.3.25f1\Editor\Unity.exe" 
)


$project = "Calculator.Unity"
$namespace = "Calculator.AppManagement.Editor"
$class = "TestScripts"
$method = "RunTests"

if ($unityEditorPath -eq $null) {
    Write-Error "No Unity Editor specified, couldn't build project."
    exit 1
}

$arguments = '-quit', '-batchmode', '-logfile -', "-projectPath ${project}", '-buildTarget WindowsStoreApps', "-executeMethod ${namespace}.${class}.${method} -ignoreCompilerErrors"

$unityProcess = Start-Process -NoNewWindow -PassThru -FilePath "$unityEditorPath" -ArgumentList $arguments
Wait-Process -InputObject $unityProcess

if ($unityProcess.ExitCode -eq 0) {
    Write-Host "`nTests were successful!"
} 
else {
    Write-Error "`nTests failed"
    $msg = $Error[0].Exception.Message
    Write-Error "`nError Message is: $msg."
}

exit $LASTEXITCODE
