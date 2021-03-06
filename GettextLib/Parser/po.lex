%namespace GettextLib.Parser
%option verbose summary out:PoLexer.cs codepage:raw stack
%visibility internal
%tokentype Tokens

digit		[0-9]
WhiteSpace	[ \t]
Eol		(\r\n?|\n)
LineComment	"#".*

%x COMMENT
%x STR

%%
	StringBuilder sbStr = new StringBuilder();

{LineComment}	{
	yylval.String = yytext.Substring(1);
	yy_push_state (COMMENT);
	return (int) Tokens.COMMENT;
}
<COMMENT>{
	{Eol} { yy_pop_state (); }
}

\"	sbStr = new StringBuilder(); BEGIN(STR);
<STR>\"	{
		/* close sequence */
		yylval.String = sbStr.ToString();		
		BEGIN(INITIAL);
		return (int) Tokens.STRING;
	}
<STR>\n	{
		/* unterminated sequence - error! */
		//throw new Exception("Unmatched string quote - must end in the same line");
	}
<STR>\\n	sbStr.Append("\n");
<STR>\\t	sbStr.Append("\t");
<STR>\\r	sbStr.Append("\r");
<STR>\\b	sbStr.Append("\b");
<STR>\\f	sbStr.Append("\f");
<STR>\\\"	sbStr.Append("\"");
<STR>\\\\	sbStr.Append("\\");
<STR>\\(.|\n)	{
 		//Console.WriteLine("what? +'" + yytext + "'");
}
<STR>[^\\\n\"]+		sbStr.Append(yytext);

"msgctxt"	return (int) Tokens.MSGCTXT;
"msgid"		return (int) Tokens.MSGID;
"msgid_plural"  return (int) Tokens.MSGID_PLURAL;
"msgstr"	return (int) Tokens.MSGSTR;
\[		return (int) Tokens.LBRACKET;
\]		return (int) Tokens.RBRACKET;

{digit}+	{ yylval.Int = int.Parse(yytext); return (int) Tokens.DIGIT; }


// Remove whitespaces.
{WhiteSpace}+	{ ; }

// End of Line
{Eol}+		{ return (int) Tokens.EOL; }


<<EOF>> {
	// End?
}

%%

	

