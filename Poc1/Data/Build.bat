setlocal
call vsvars32.bat

msbuild build.xml /t:Build
endlocal
