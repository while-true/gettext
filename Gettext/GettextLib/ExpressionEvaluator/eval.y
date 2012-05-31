%namespace GettextLib.ExpressionEvaluator
%output=EvalParser.cs
%visibility internal
%start Source
%partial
%using GettextLib.ExpressionEvaluator

%union {
	internal int Int;
	internal string StringId;
	
	internal Assignment Assignment;
	internal List<Assignment> Assignments;
	
	internal Expr Expr;
	
	internal Literal Literal;
}

%token <Int> DIGIT
%token EQUALS
%token NOTEQUALS
%token GT
%token LT
%token GTEQUALS
%token LTEQUALS
%token SEMICOLON
%token ASSIGN
%token QUESTIONMARK
%token COLON
%token PERCENT
%token <StringId> IDENTIFIER
%token OR
%token AND
%token LEFTPAR
%token RIGHTPAR
%token MINUS
%token PLUS
%token DIV
%token MUL
%token NOT

%type <Assignment> AssigmentExpression
%type <Assignments> AssigmentExpressions
%type <Expr> Expr
%type <Literal> Literal

%right QUESTIONMARK
%left OR
%left AND
%left EQUALS NOTEQUALS
%left LT GT LTEQUALS GTEQUALS
%left MINUS PLUS
%left MUL DIV PERCENT
%right NOT


%%

Source
	: AssigmentExpressions EOF { Script = new Script(); Script.Assignments.AddRange($1); }
	;

AssigmentExpressions
	: AssigmentExpression { $$ = new List<Assignment>(); $$.Add($1); }
	| AssigmentExpressions AssigmentExpression { $1.Add($2); $$ = $1; }
	;
	
AssigmentExpression
	: IDENTIFIER ASSIGN Expr SEMICOLON { $$ = new Assignment($1, $3); }
	;

Expr
	: Expr QUESTIONMARK Expr COLON Expr { $$ = new ExprIf($1, $3, $5); }
	
	| Expr OR Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Or); }
	| Expr AND Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.And); }
	
	| Expr EQUALS Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Equals); }
	| Expr NOTEQUALS Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.NotEquals); }
	
	| Expr LT Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.LessThan); }
	| Expr GT Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.GreaterThan); }
	| Expr LTEQUALS Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.LessThanOrEquals); }
	| Expr GTEQUALS Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.GreaterThanOrEquals); }
	
	| Expr PLUS Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.And); }
	| Expr MINUS Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Minus); }
	
	| Expr MUL Expr { ; }
	| Expr DIV Expr { ; }
	| Expr PERCENT Expr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Modulo); }
	
	| Expr NOT Expr { ; }
	
	| Literal { $$ = $1; }
	
	| LEFTPAR Expr RIGHTPAR { $$ = new ExprWrapper($2); }
	;	

Literal
	: IDENTIFIER { $$ = new LiteralVar($1); }
	| DIGIT { $$ = new LiteralNumber($1); }
	;
%%

internal GettextLib.ExpressionEvaluator.Script Script { get; private set; }

public Parser(Scanner scn) : base(scn) { }                                                                                              