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


%token SEMICOLON


%token <Int> DIGIT
%token <StringId> IDENTIFIER

%token LEFTPAR RIGHTPAR
%right QUESTIONMARK
%token COLON
%left OR
%left AND
%left EQUALS NOTEQUALS
%left LT GT LTEQUALS GTEQUALS
%left MINUS PLUS
%left MUL DIV PERCENT
%right NOT
%right ASSIGN


%type <Assignment> AssigmentExpression
%type <Assignments> AssigmentExpressions
%type <Expr> Expr ExprOr ExprAnd ExprEqu ExprRel ExprMod ExprLiteral
%type <Literal> Literal


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
	: ExprOr QUESTIONMARK Expr COLON Expr { $$ = new ExprIf($1, $3, $5); }
	| ExprOr { $$ = $1; }
	;

ExprOr
	: ExprAnd OR ExprOr { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Or); }
	| ExprAnd { $$ = $1; }
	;

ExprAnd
	: ExprEqu AND ExprAnd { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.And); }
	| ExprEqu { $$ = $1; }
	;
	
ExprEqu
	: ExprRel EQUALS ExprRel { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Equals); }
	| ExprRel NOTEQUALS ExprRel { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.NotEquals); }
	| ExprRel { $$ = $1; }
	;

ExprRel
	: ExprMod LT ExprMod { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.LessThan); }
	| ExprMod GT ExprMod { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.GreaterThan); }
	| ExprMod LTEQUALS ExprMod { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.LessThanOrEquals); }
	| ExprMod GTEQUALS ExprMod { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.GreaterThanOrEquals); }
	| ExprMod { $$ = $1; }
	;

ExprMod
	: ExprLiteral PERCENT ExprLiteral { $$ = new ExprTwo($1, $3, ExprTwo.OpEnum.Modulo); }
	| ExprLiteral { $$ = $1; }
	;
	
ExprLiteral
	: Literal { $$ = $1; }
	| LEFTPAR Expr RIGHTPAR { $$ = new ExprWrapper($2); }
	;

Literal
	: IDENTIFIER { $$ = new LiteralVar($1); }
	| DIGIT { $$ = new LiteralNumber($1); }
	;
%%

internal GettextLib.ExpressionEvaluator.Script Script { get; private set; }

public Parser(Scanner scn) : base(scn) { }                                                                                              