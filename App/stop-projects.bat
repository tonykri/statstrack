@echo off
echo Stopping all .NET applications...
taskkill /IM dotnet.exe /F
echo All .NET applications stopped.
pause