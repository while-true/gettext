@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITCODE
)

set __gettext_current_dir=%CD%\
set __gettext_tools_dir=%~dp0

copy /Y %__gettext_tools_dir%\bin\GettextExtractorApp.exe %1\GettextExtractorApp.exe

%1\GettextExtractorApp.exe %1

DEL %1\GettextExtractorApp.exe


:QUITCODE