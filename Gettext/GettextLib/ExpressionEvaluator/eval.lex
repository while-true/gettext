%namespace GettextLib.ExpressionEvaluator
%option verbose summary nofiles out:EvalLexer.cs codePage:utf-8 unicode
%visibility internal
%tokentype Tokens

digit	[0-9]
ws	[ \t]
eol	(\r\n?|\n)
identifier	[a-zA-Z]+

%%

"=="	{ return (int) Tokens.EQUALS; }
"!="	{ return (int) Tokens.NOTEQUALS; }
"="	{ return (int) Tokens.ASSIGN; }
"%"	{ return (int) Tokens.PERCENT; }
":"	{ return (int) Tokens.COLON; }
";"	{ return (int) Tokens.SEMICOLON; }
">"	{ return (int) Tokens.GT; }
"<"	{ return (int) Tokens.LT; }
">="	{ return (int) Tokens.GTEQUALS; }
"<="	{ return (int) Tokens.LTEQUALS; }
"?"	{ return (int) Tokens.QUESTIONMARK; }
"||"	{ return (int) Tokens.OR; }
"&&"	{ return (int) Tokens.AND; }
"("	{ return (int) Tokens.LEFTPAR; }
")"	{ return (int) Tokens.RIGHTPAR; }
"-"	{ return (int) Tokens.MINUS; }
"+"	{ return (int) Tokens.PLUS; }
"*"	{ return (int) Tokens.MUL; }
"/"	{ return (int) Tokens.DIV; }
"!"	{ return (int) Tokens.NOT; }

{digit}+	{ yylval.Int = int.Parse(yytext); return (int) Tokens.DIGIT; }
{identifier}	{ yylval.StringId = yytext; return (int) Tokens.IDENTIFIER; }

{ws}+	{ ; }
{eol}+	{ ; }

<<EOF>> { ; }

%%
