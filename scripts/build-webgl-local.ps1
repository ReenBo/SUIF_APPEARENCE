param (
    [string]$UnityEditorPath = "E:\UnityEditor\6000.6.7f1\Editor\Unity.exe",
    [string]$ProjectPath = $PSScriptRoot + "\..",
    [string]$OutputPath = "Builds/WebGL",
    [switch]$DevelopmentBuild
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $UnityEditorPath)) {
    Write-Error "Unity Editor not found at: $UnityEditorPath"
    exit 1
}

if (-not (Test-Path "Builds")) {
    New-Item -ItemType Directory -Path "Builds" | Out-Null
}

Write-Host "Starting WebGL Build for SUIF_APPEARENCE..." -ForegroundColor Cyan

$extraArgs = @()
if ($DevelopmentBuild) {
    $extraArgs += "-developmentBuild"
}

$unityArgs = @(
    "-quit",
    "-batchmode",
    "-projectPath", (Resolve-Path $ProjectPath).Path,
    "-executeMethod", "SUIF.Editor.Build.WebGLBuilder.BuildFromCommandLine",
    "-outputFolder", $OutputPath,
    "-logFile", "Builds/webgl_build.log"
) + $extraArgs

Write-Host "Executing Unity batchmode: $UnityEditorPath $($unityArgs -join ' ')" -ForegroundColor DarkGray
& $UnityEditorPath $unityArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "Unity WebGL Build failed with exit code $LASTEXITCODE. Check Builds/webgl_build.log for details."
    exit $LASTEXITCODE
}

Write-Host "WebGL Build completed successfully at $OutputPath!" -ForegroundColor Green

