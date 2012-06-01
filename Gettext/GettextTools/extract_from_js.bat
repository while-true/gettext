@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITCODE
)

call extract_vars.bat

find %1 -iname "*.js" | sed "s/\//\\/g" > files.txt
xgettext -f files.txt --from-code=UTF-8 --keyword=_ -L C --omit-header -o messages-js.pot

:QUITCODE