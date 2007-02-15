call "%VS71COMNTOOLS%vsvars32.bat"
devenv RB.sln /clean Debug
devenv RB.sln /clean Release
devenv RB.sln /clean Documentation
del .\RbEngine\Documentation\NDoc /s /q
del .\RbEngine\Documentation\Errors.txt
del .\RbEngine\Documentation\RbEngine.xml
