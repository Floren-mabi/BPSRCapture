# Update-Version.ps1
$assemblyInfoPath = "Properties\AssemblyInfo.cs"
$buildFile = "buildnumber.txt"

# ビルド番号
$buildNumber = 0
if (Test-Path $buildFile) { $buildNumber = [int](Get-Content $buildFile) }
$buildNumber++
$version = "1.3.$buildNumber.0"

# メタデータ
$title = "BPSRCapture"
$product = "BPSRCapture"
$copyright = "©2025 Floren_mabi"  # ← ここは正しい

# AssemblyInfo.cs 内容
$content = @"
using System.Reflection;

[assembly: AssemblyTitle("$title")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Floren")]
[assembly: AssemblyProduct("$product")]
[assembly: AssemblyCopyright("$copyright")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("$version")]
[assembly: AssemblyFileVersion("$version")]
[assembly: AssemblyInformationalVersion("$version")]
"@

# フォルダ作成
if (!(Test-Path "Properties")) { New-Item -ItemType Directory -Path "Properties" }

# BOM付き UTF-8 で保存（これが重要！）
$utf8WithBom = New-Object System.Text.UTF8Encoding $true
$bytes = $utf8WithBom.GetBytes($content)
[System.IO.File]::WriteAllBytes($assemblyInfoPath, $bytes)

# ビルド番号保存
Set-Content -Path $buildFile -Value $buildNumber

Write-Host "バージョン更新: $version (© 正常保存)" -ForegroundColor Green