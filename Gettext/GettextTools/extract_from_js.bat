@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITCODE
)

set __gettext_current_dir=%CD%\
set __gettext_tools_dir=%~dp0

call %__gettext_tools_dir%extract_vars.bat

%__gettext_tools_dir%find %1 -iname "*.js" | %__gettext_tools_dir%sed "s/\//\\/g" > %__gettext_current_dir%files.txt
%__gettext_tools_dir%xgettext -f %__gettext_current_dir%files.txt --from-code=UTF-8 --keyword=_ --keyword=_:1,2 -L C --omit-header -o %__gettext_current_dir%messages-js.pot

:QUITCODE