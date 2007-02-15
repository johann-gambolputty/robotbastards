
call BuildDocumentationSettings.bat

devenv ../RbEngine.csproj /build Documentation > Errors.txt

ndocconsole -project="./RbEngine.ndoc" >> Errors.txt


copy .\NDoc\*.chm

REM Delete all HTML noodles
del .\NDoc\*.* /q
