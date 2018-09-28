REM set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v3.5
set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call %msBuildDir%\msbuild.exe WindowsOcr.sln /p:Configuration=Release /p:Platform=x86
set msBuildDir=