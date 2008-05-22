REM
REM This builds all rendering implementations for a given configuration, then copies them to a destination directory
REM
REM Usage:
REM	BuildAndCopy.bat [Configuration] [DestinationDirectory]
REM
REM Where [Configuration] is usually either "Debug" or "Release", and [DestinationDirectory] is
REM the path to the destination where all the rendering assemblies are copied.
REM
REM For example,
REM	BuildAndCopy Debug .\Temp
REM will build all the debug rendering assemblies into a directory structure looking like this:
REM Debug\Rendering\OpenGl\[all opengl assemblies]
REM Debug\Rendering\Dx9.0\[all dx9.0 assemblies]
REM Next, this directory structure will be copied into ".\Temp":
REM Temp\Rendering\OpenGl\[all opengl assemblies]
REM ...
REM

setlocal

@echo ON

echo Building rendering implementations...
call vsvars32

msbuild rb.rendering.implementations.csproj /t:Build /p:Configuration=%1

mkdir Rendering
xcopy "..\..\bin\%1\Rendering" "%2\Rendering" /e /s /i /y

endlocal
