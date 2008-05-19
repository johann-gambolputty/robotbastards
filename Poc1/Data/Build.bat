setlocal
call vsvars32.bat

msbuild build.xml /t:Build /l:FileLogger,Microsoft.Build.Engine;logfile=BuildOutput.txt

endlocal
