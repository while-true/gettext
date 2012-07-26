@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITCODE
)

set __gettext_current_dir=%CD%\
set __gettext_tools_dir=%~dp0

call %__gettext_tools_dir%extract_vars.bat

%__gettext_tools_dir%find %1 -iname "*.cs" | %__gettext_tools_dir%sed "s/\//\\/g" > %__gettext_current_dir%files.txt
%__gettext_tools_dir%xgettext --force-po -f %__gettext_current_dir%files.txt %xgettext_options% -o %__gettext_current_dir%messages-code.pot

DEL %__gettext_current_dir%files.txt

:QUITCODE