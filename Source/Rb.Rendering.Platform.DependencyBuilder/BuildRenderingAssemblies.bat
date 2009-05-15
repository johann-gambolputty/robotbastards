REM Builds rendering assemblies for this platform and configuration

call "%VS80COMNTOOLS%vsvars32.bat"

devenv ..\..\Rb.Rendering.OpenGl.sln /build %1