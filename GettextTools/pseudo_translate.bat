@echo off
IF [%1]==[] (
	echo Missing input - POT file
	goto QUIT
)
IF [%2]==[] (
	echo Missing output - PO file
	goto QUIT
)

set __gettext_current_dir=%CD%\
set __gettext_tools_dir=%~dp0

REM prepare an english translation as the basis

%__gettext_tools_dir%\msginit.exe --locale=en_US -o %2 -i %1

REM convert to utf-8

%__gettext_tools_dir%\msgconv.exe --to-code=UTF-8 -o %2 %2

%__gettext_tools_dir%\msgen.exe -o %2 --force-po %2

%__gettext_tools_dir%\translation-tools\podebug.exe --rewrite=bracket %2 %2


:QUIT