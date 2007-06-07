call vsvars32.bat
devenv RB.sln /build Debug > BuildOutput.txt
devenv RB.sln /build Release >> BuildOutput.txt
REM devenv RB.sln /build Documentation >> BuildOutput.txt