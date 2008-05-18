setlocal
call vsvars32.bat

msbuild build.xml /t:Rebuild
endlocal
