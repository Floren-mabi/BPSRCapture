@echo off
chcp 65001
powershell -ExecutionPolicy Bypass -File Update-Version.ps1
rem dotnet publish -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o "C:\Users\kazks\source\repos\BPSRCapture\BPSRCapture\Publish"
dotnet publish -p:PublishProfile=FolderProfile
exit /B
