%namespace Gettext.PoScanner
%output=PoParser.cs
%visibility internal
%start Catalog
%partial

%union {
	public string String;
	public int Int;
	
	public GettextMvcLib.Catalog.MultiLineString multiLineString;
	
	public GettextMvcLib.Catalog.MultiLineString comments;
	
	public GettextMvcLib.Catalog.GettextCatalog Catalog;
	
	public List<GettextMvcLib.Catalog.Translation> Messages;
	public GettextMvcLib.Catalog.Translation Message;
	
	public GettextMvcLib.Catalog.Translation.TranslationString MessageTranslation;
	public List<GettextMvcLib.Catalog.Translation.TranslationString> MessageTranslations;
	
	public GettextMvcLib.Catalog.MultiLineString MessageContext;
	
	public GettextMvcLib.Catalog.Translation MessageId;
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
				$$ = new GettextMvcLib.Catalog.GettextCatalog();
				$$.AddTranslations($1);
				
				Catalog = $$;
			}
	EOF
	;
	
MessageBlocks
	: MessageBlock { $$ = new List<GettextMvcLib.Catalog.Translation>(); $$.Add($1); }
	| MessageBlocks MessageBlock { $1.Add($2); $$ = $1; }
	;

MessageBlock
	: MessageComments MessageContext MessageId MessageTranslations 
		{
			$$ = new GettextMvcLib.Catalog.Translation();
			if ($1 != null)	$$.Comment = $1;
			if ($2 != null) $$.MessageContext = $2;
			
			$$.MessageId = $3.MessageId;
			$$.MessageIdPlural = $3.MessageIdPlural;
			
			$$.MessageTranslations = $4;
		}
	;

MessageComments
	: Comments { $$ = $1; }
	| { $$ = null; }
	;

MessageId
	: MSGID MultiLineString 
		{
			$$ = new GettextMvcLib.Catalog.Translation();
			$$.MessageId = $2;
		}
	| MSGID MultiLineString MSGID_PLURAL MultiLineString
		{
			$$ = new GettextMvcLib.Catalog.Translation();
			$$.MessageId = $2;
			$$.MessageIdPlural = $4;
		}
	;

MessageContext
	:	MSGCTXT MultiLineString { $$ = $2; }
	| { $$ = null; }
	;

MessageTranslations
	:	MessageTranslation { $$ = new List<GettextMvcLib.Catalog.Translation.TranslationString>(); $$.Add($1); }
	|	MessageTranslations MessageTranslation { $1.Add($2); $$ = $1; }
	;

MessageTranslation
	:	MSGSTR MultiLineString
		{
			$$ = new GettextMvcLib.Catalog.Translation.TranslationString();
			$$.Message = $2;
			$$.Index = 0;
		}
	|	MSGSTR LBRACKET DIGIT RBRACKET MultiLineString
		{
			$$ = new GettextMvcLib.Catalog.Translation.TranslationString();
			$$.Message = $5;
			$$.Index = $3;
		}
	;

Comments 
	: COMMENT { $$ = new GettextMvcLib.Catalog.MultiLineString(); $$.AddLine($1); }
	| Comments COMMENT { $1.AddLine($2); $$ = $1; }
	;

MultiLineString
	: STRING EOL	{
				$$ = new GettextMvcLib.Catalog.MultiLineString();
				$$.AddLine($1);
			}
	| MultiLineString STRING EOL	{
						$1.AddLine($2);
						$$ = $1;
					}
	;

%%

public GettextMvcLib.Catalog.GettextCatalog Catalog { get; private set; }

public Parser(Scanner scn) : base(scn) { }


