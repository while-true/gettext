%namespace GettextLib.Parser
%output=PoParser.cs
%visibility internal
%start Catalog
%partial

%union {
	public string String;
	public int Int;
	
	public GettextLib.Catalog.MultiLineString multiLineString;
	
	public GettextLib.Catalog.MultiLineString comments;
	
	public GettextLib.Catalog.GettextCatalog Catalog;
	
	public List<GettextLib.Catalog.Translation> Messages;
	public GettextLib.Catalog.Translation Message;
	
	public GettextLib.Catalog.Translation.TranslationString MessageTranslation;
	public List<GettextLib.Catalog.Translation.TranslationString> MessageTranslations;
	
	public GettextLib.Catalog.MultiLineString MessageContext;
	
	public GettextLib.Catalog.Translation MessageId;
}

%token <String> STRING
%token <Int> DIGIT
%token MSGID
%token MSGID_PLURAL
%token MSGSTR
%token MSGCTXT
%token LBRACKET
%token RBRACKET
%token <String> COMMENT
%token EOL

%type <multiLineString> MultiLineString
%type <Catalog> Catalog
%type <Messages> MessageBlocks
%type <Message> MessageBlock

%type <comments> Comments
%type <comments> MessageComments



%type <MessageTranslation> MessageTranslation
%type <MessageTranslations> MessageTranslations

%type <MessageContext> MessageContext
%type <MessageId> MessageId

%%

Catalog
	: MessageBlocks	{
				$$ = new GettextLib.Catalog.GettextCatalog();
				
				if ($1 != null) {
					$$.AddTranslations($1);
				}
				
				Catalog = $$;
			}
	;
	
MessageBlocks
	: MessageBlock { 
		$$ = new List<GettextLib.Catalog.Translation>(); 
		if ($1 != null) $$.Add($1);
	}
	| MessageBlocks MessageBlock { if ($2 != null) { $1.Add($2); } $$ = $1; }
	;

MessageBlock
	: MessageComments MessageContext MessageId MessageTranslations 
		{
			$$ = new GettextLib.Catalog.Translation();
			if ($1 != null)	$$.Comment = $1;
			if ($2 != null) $$.MessageContext = $2;
			
			$$.MessageId = $3.MessageId;
			$$.MessageIdPlural = $3.MessageIdPlural;
			
			$$.MessageTranslations = $4;
		}
	| Comments EOF
	| Comments EOL
	;

MessageComments
	: Comments { $$ = $1; }
	| { $$ = null; }
	;

MessageId
	: MSGID MultiLineString 
		{
			$$ = new GettextLib.Catalog.Translation();
			$$.MessageId = $2;
		}
	| MSGID MultiLineString MSGID_PLURAL MultiLineString
		{
			$$ = new GettextLib.Catalog.Translation();
			$$.MessageId = $2;
			$$.MessageIdPlural = $4;
		}
	;

MessageContext
	:	MSGCTXT MultiLineString { $$ = $2; }
	| { $$ = null; }
	;

MessageTranslations
	:	MessageTranslation { $$ = new List<GettextLib.Catalog.Translation.TranslationString>(); $$.Add($1); }
	|	MessageTranslations MessageTranslation { $1.Add($2); $$ = $1; }
	;

MessageTranslation
	:	MSGSTR MultiLineString
		{
			$$ = new GettextLib.Catalog.Translation.TranslationString();
			$$.Message = $2;
			$$.Index = 0;
		}
	|	MSGSTR LBRACKET DIGIT RBRACKET MultiLineString
		{
			$$ = new GettextLib.Catalog.Translation.TranslationString();
			$$.Message = $5;
			$$.Index = $3;
		}
	;

Comments 
	: COMMENT { $$ = new GettextLib.Catalog.MultiLineString(); $$.AddLine($1); }
	| Comments COMMENT { $1.AddLine($2); $$ = $1; }
	;

MultiLineString
	: STRING EOL {
				$$ = new GettextLib.Catalog.MultiLineString();
				$$.AddLine($1);
		     }
	| STRING EOF {
				$$ = new GettextLib.Catalog.MultiLineString();
				$$.AddLine($1);	
	             }
	| MultiLineString STRING EOL	{
						$1.AddLine($2);
						$$ = $1;
					}
	| MultiLineString STRING EOF	{
						$1.AddLine($2);
						$$ = $1;
					}
	;

%%

public GettextLib.Catalog.GettextCatalog Catalog { get; private set; }

public Parser(Scanner scn) : base(scn) { }


