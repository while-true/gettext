%namespace Gettext.PoScanner
%option verbose summary noparser nofiles out:PoScanner.cs codePage:utf-8

digit	[0-9]

%x COMMENT
%x STR

%%
	StringBuilder sbStr = new StringBuilder();
	string	commentStr = "";


"#"	BEGIN(COMMENT);
<COMMENT>[^\n]*	commentStr = yytext;
<COMMENT>\n	BEGIN(INITIAL);

\"	sbStr = new StringBuilder(); BEGIN(STR);
<STR>\"	{
		/* close sequence */
		Console.WriteLine("matched string: " + sbStr.ToString());
		BEGIN(INITIAL);
	}
<STR>\n	{
		/* unterminated sequence - error! */
		throw new Exception("Unmatched string quote - must end in the same line");
	}
<STR>\\n	sbStr.Append("\n");
<STR>\\t	sbStr.Append("\t");
<STR>\\r	sbStr.Append("\r");
<STR>\\b	sbStr.Append("\b");
<STR>\\f	sbStr.Append("\f");
<STR>\\(.|\n)	{
 		Console.WriteLine("what?");
}
<STR>[^\\\n\"]+	{
		//Console.WriteLine("copy?");
		sbStr.Append(yytext);
}


"msgctxt"	/* ... */
"msgid"		/* ... */
"msgstr"	/* ... */
\[	/* ... */
\]	/* ... */


{digit}+	{ Console.WriteLine("number: " + yytext); }

[ \t\n\r]	/* skip whitespace */


<<EOF>> {
	// End?
}

%%

