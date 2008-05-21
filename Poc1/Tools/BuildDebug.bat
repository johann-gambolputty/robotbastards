setlocal

call vsvars32.bat
devenv Tools.sln /build Debug > BuildOutput.txt

endlocal