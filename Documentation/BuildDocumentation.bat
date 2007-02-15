
call BuildDocumentationSettings.bat

devenv ../Rb.sln /build Documentation > Errors.txt

ndocconsole -project="./Rb.ndoc" >> Errors.txt


copy .\NDoc\*.chm

REM Delete all HTML noodles
del .\NDoc\*.* /q
