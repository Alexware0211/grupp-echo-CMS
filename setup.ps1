<#
  Kvarter - one-time project setup (SCRUM-16, SCRUM-17, SCRUM-18)
  Run from this folder in PowerShell:   .\setup.ps1
  If scripts are blocked:               powershell -ExecutionPolicy Bypass -File .\setup.ps1
#>
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

function Step($text) { Write-Host "`n==> $text" -ForegroundColor Cyan }
function Run([string]$cmd) {
    Write-Host "    $cmd" -ForegroundColor DarkGray
    Invoke-Expression $cmd
    if ($LASTEXITCODE -ne 0) { throw "Command failed ($LASTEXITCODE): $cmd" }
}

$web = 'src/Kvarter.Web'

# ---------------------------------------------------------------- SCRUM-17
if (-not (Test-Path "$web/Kvarter.Web.csproj")) {
    Step 'Installing the latest Umbraco templates'
    Run 'dotnet new install Umbraco.Templates --force'

    Step 'Creating the Umbraco 17 LTS project (SQLite for local development)'
    # Our Views/ and wwwroot/ files already exist in the folder; the template only adds its own files.
    Run "dotnet new umbraco -n Kvarter.Web -o $web --release LTS --development-database-type SQLite --package-management PerProject --minimal-gitignore"

    Step 'Creating the solution'
    Run 'dotnet new sln -n Kvarter'
    $sln = Get-ChildItem -Filter 'Kvarter.sln*' | Select-Object -First 1
    Run "dotnet sln $($sln.Name) add $web/Kvarter.Web.csproj"
} else {
    Write-Host 'Project already exists - skipping creation.' -ForegroundColor Yellow
}

# ---------------------------------------------------------------- SCRUM-18
Step 'Adding uSync (version 17 matches Umbraco 17)'
$csproj = Get-Content "$web/Kvarter.Web.csproj" -Raw
if ($csproj -notmatch 'Include="uSync"') {
    dotnet add $web package uSync --version '17.*'
    if ($LASTEXITCODE -ne 0) {
        Write-Host '    Floating version not accepted - using uSync 17.0.2' -ForegroundColor Yellow
        Run "dotnet add $web package uSync --version 17.0.2"
    }
}

Step 'Configuring uSync: export on save, import at startup'
$appsettingsPath = "$web/appsettings.json"
$json = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
$usync = [pscustomobject]@{
    Settings = [pscustomobject]@{
        ImportAtStartup = 'All'   # pull + restart = teammates' doc types appear
        ExportOnSave    = 'All'   # every backoffice change is written to uSync/ files
    }
}
$json | Add-Member -NotePropertyName 'uSync' -NotePropertyValue $usync -Force
$json | ConvertTo-Json -Depth 32 | Set-Content $appsettingsPath -Encoding utf8

Step 'Building'
Run "dotnet build $web"

# ---------------------------------------------------------------- SCRUM-16
Step 'Initialising Git'
if (-not (Test-Path '.git')) {
    Run 'git init -b main'
}
Run 'git add -A'
git diff --cached --quiet
if ($LASTEXITCODE -ne 0) { Run 'git commit -m "Initial Umbraco 17 project with uSync and base layout"' }

if (Get-Command gh -ErrorAction SilentlyContinue) {
    Step 'Creating the GitHub repository'
    $remote = git remote 2>$null
    if (-not $remote) {
        Run 'gh repo create Kvarter --private --source . --remote origin --push'
    } else {
        Run 'git push -u origin main'
    }
    Write-Host "`nAdd your teammates:  gh repo edit --add-collaborator <github-username>   (or GitHub > Settings > Collaborators)" -ForegroundColor Green
} else {
    Write-Host @"

GitHub CLI (gh) not found. Either install it (winget install GitHub.cli, then gh auth login and re-run this script),
or create an empty repo called Kvarter on github.com and run:
    git remote add origin https://github.com/<your-username>/Kvarter.git
    git push -u origin main
"@ -ForegroundColor Yellow
}

Step 'Done'
Write-Host "Start the site:  dotnet run --project $web" -ForegroundColor Green
Write-Host 'Then open the URL it prints, finish the installer (create your admin user), and follow README.md > First run.' -ForegroundColor Green
