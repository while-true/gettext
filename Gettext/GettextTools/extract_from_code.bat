@echo off
IF [%1]==[] (
	echo Missing directory
	goto QUITCODE
)
find %1 -iname "*.cs" | sed "s/\//\\/g" > files.txt
xgettext -f files.txt --from-code=UTF-8 --language=C# --keyword=_ --keyword=_:1,2 -o messages-code.pot

:QUITCODE