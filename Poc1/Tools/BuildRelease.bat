setlocal

call vsvars32.bat
devenv Tools.sln /build Release > BuildOutput.txt

endlocal
