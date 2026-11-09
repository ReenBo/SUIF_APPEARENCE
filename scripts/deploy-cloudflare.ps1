param (
    [string]$AccountId = "aa6c1dbefe7964a02d6bc7b2b4c0d3a7",
    [string]$ApiToken = $env:CLOUDFLARE_API_TOKEN,
    [string]$ProjectName = "suif-showcase",
    [string]$BuildPath = "Builds/WebGL",
    [string]$Branch = "develop"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $BuildPath)) {
    Write-Error "Build directory '$BuildPath' not found. Run WebGL build first."
    exit 1
}

if (-not $ApiToken) {
    Write-Host "Cloudflare API Token not specified in environment variable CLOUDFLARE_API_TOKEN." -ForegroundColor Yellow
    $ApiToken = Read-Host -Prompt "Enter your Cloudflare API Token"
}

Write-Host "Deploying $BuildPath to Cloudflare Pages project '$ProjectName'..." -ForegroundColor Cyan

$wranglerCmd = Get-Command wrangler -ErrorAction SilentlyContinue
if ($wranglerCmd) {
    $env:CLOUDFLARE_ACCOUNT_ID = $AccountId
    $env:CLOUDFLARE_API_TOKEN = $ApiToken
    & wrangler pages deploy $BuildPath --project-name $ProjectName --branch $Branch
} else {
    Write-Host "Wrangler CLI not found locally. To deploy via Wrangler, run: npx wrangler pages deploy $BuildPath --project-name $ProjectName" -ForegroundColor Yellow
}

