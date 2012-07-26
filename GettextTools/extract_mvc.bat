@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITMVC
)

set __gettext_current_dir=%CD%\
set __gettext_tools_dir=%~dp0

call %__gettext_tools_dir%extract_from_code.bat %1
call %__gettext_tools_dir%extract_from_views.bat %1
call %__gettext_tools_dir%extract_from_js.bat %1
call %__gettext_tools_dir%extract_data_annotations.bat %1\bin\

%__gettext_tools_dir%msgcat messages-code.pot messages-data-annotations.pot messages-views.pot messages-js.pot > messages.pot

:QUITMVC