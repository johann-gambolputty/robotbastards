call vsvars32.bat
devenv "RB 1.0.sln" /build Debug > BuildOutput.txt
devenv "RB 1.0.sln" /build Release >> BuildOutput.txt
