@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITCODE
)

call extract_vars.bat

find %1 -iname "*.cs" | sed "s/\//\\/g" > files.txt
xgettext -f files.txt %xgettext_options% -o messages-code.pot

:QUITCODE