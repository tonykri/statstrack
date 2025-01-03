@echo off
echo Starting to run all .NET projects...

for /d %%d in (*) do (
    echo.
    echo Checking directory: %%d
    if exist "%%d\*.csproj" (
        echo Found .NET project in %%d - running...
        pushd %%d
        start cmd /c "dotnet run && pause"
        popd
    ) else if exist "%%d\*.fsproj" (
        echo Found .NET project in %%d - running...
        pushd %%d
        start cmd /c "dotnet run && pause"
        popd
    )
)

echo.
echo Started all projects.
pause