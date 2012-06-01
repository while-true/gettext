@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITMVC
)

call extract_from_code.bat %1
call extract_from_views.bat %1
call extract_from_js.bat %1
GettextExtractorApp.exe %1


msgcat messages-code.pot messages-data-annotations.pot messages-views.pot messages-js.pot > messages.pot

:QUITMVC