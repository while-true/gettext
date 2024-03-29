// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.0
// Machine:  GARFIELD
// DateTime: 27.7.2012 11:55:58
// UserName: Rudi
// Input file <po.y - 27.7.2012 11:55:56>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace GettextLib.Parser
{
internal enum Tokens {
    error=1,EOF=2,STRING=3,DIGIT=4,MSGID=5,MSGID_PLURAL=6,
    MSGSTR=7,MSGCTXT=8,LBRACKET=9,RBRACKET=10,COMMENT=11,EOL=12};

internal partial struct ValueType
{
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
// Abstract base class for GPLEX scanners
internal abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
internal class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

internal partial class Parser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[24];
  private static State[] states = new State[35];
  private static string[] nonTerms = new string[] {
      "Catalog", "MultiLineString", "MessageBlocks", "MessageBlock", "Comments", 
      "MessageComments", "MessageTranslation", "MessageTranslations", "MessageContext", 
      "MessageId", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{11,33,8,-9,5,-9},new int[]{-1,1,-3,3,-4,34,-6,5,-5,29});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{11,33,2,-2,8,-9,5,-9},new int[]{-4,4,-6,5,-5,29});
    states[4] = new State(-4);
    states[5] = new State(new int[]{8,27,5,-13},new int[]{-9,6});
    states[6] = new State(new int[]{5,23},new int[]{-10,7});
    states[7] = new State(new int[]{7,10},new int[]{-8,8,-7,22});
    states[8] = new State(new int[]{7,10,11,-5,8,-5,5,-5,2,-5},new int[]{-7,9});
    states[9] = new State(-15);
    states[10] = new State(new int[]{9,15,3,19},new int[]{-2,11});
    states[11] = new State(new int[]{3,12,7,-16,11,-16,8,-16,5,-16,2,-16});
    states[12] = new State(new int[]{12,13,2,14});
    states[13] = new State(-22);
    states[14] = new State(-23);
    states[15] = new State(new int[]{4,16});
    states[16] = new State(new int[]{10,17});
    states[17] = new State(new int[]{3,19},new int[]{-2,18});
    states[18] = new State(new int[]{3,12,7,-17,11,-17,8,-17,5,-17,2,-17});
    states[19] = new State(new int[]{12,20,2,21});
    states[20] = new State(-20);
    states[21] = new State(-21);
    states[22] = new State(-14);
    states[23] = new State(new int[]{3,19},new int[]{-2,24});
    states[24] = new State(new int[]{6,25,3,12,7,-10});
    states[25] = new State(new int[]{3,19},new int[]{-2,26});
    states[26] = new State(new int[]{3,12,7,-11});
    states[27] = new State(new int[]{3,19},new int[]{-2,28});
    states[28] = new State(new int[]{3,12,5,-12});
    states[29] = new State(new int[]{11,30,2,31,12,32,8,-8,5,-8});
    states[30] = new State(-19);
    states[31] = new State(-6);
    states[32] = new State(-7);
    states[33] = new State(-18);
    states[34] = new State(-3);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-11, new int[]{-1,2});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-4});
    rules[4] = new Rule(-3, new int[]{-3,-4});
    rules[5] = new Rule(-4, new int[]{-6,-9,-10,-8});
    rules[6] = new Rule(-4, new int[]{-5,2});
    rules[7] = new Rule(-4, new int[]{-5,12});
    rules[8] = new Rule(-6, new int[]{-5});
    rules[9] = new Rule(-6, new int[]{});
    rules[10] = new Rule(-10, new int[]{5,-2});
    rules[11] = new Rule(-10, new int[]{5,-2,6,-2});
    rules[12] = new Rule(-9, new int[]{8,-2});
    rules[13] = new Rule(-9, new int[]{});
    rules[14] = new Rule(-8, new int[]{-7});
    rules[15] = new Rule(-8, new int[]{-8,-7});
    rules[16] = new Rule(-7, new int[]{7,-2});
    rules[17] = new Rule(-7, new int[]{7,9,4,10,-2});
    rules[18] = new Rule(-5, new int[]{11});
    rules[19] = new Rule(-5, new int[]{-5,11});
    rules[20] = new Rule(-2, new int[]{3,12});
    rules[21] = new Rule(-2, new int[]{3,2});
    rules[22] = new Rule(-2, new int[]{-2,3,12});
    rules[23] = new Rule(-2, new int[]{-2,3,2});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // Catalog -> MessageBlocks
{
				CurrentSemanticValue.Catalog = new GettextLib.Catalog.GettextCatalog();
				
				if (ValueStack[ValueStack.Depth-1].Messages != null) {
					CurrentSemanticValue.Catalog.AddTranslations(ValueStack[ValueStack.Depth-1].Messages);
				}
				
				Catalog = CurrentSemanticValue.Catalog;
			}
        break;
      case 3: // MessageBlocks -> MessageBlock
{ 
		CurrentSemanticValue.Messages = new List<GettextLib.Catalog.Translation>(); 
		if (ValueStack[ValueStack.Depth-1].Message != null) CurrentSemanticValue.Messages.Add(ValueStack[ValueStack.Depth-1].Message);
	}
        break;
      case 4: // MessageBlocks -> MessageBlocks, MessageBlock
{ if (ValueStack[ValueStack.Depth-1].Message != null) { ValueStack[ValueStack.Depth-2].Messages.Add(ValueStack[ValueStack.Depth-1].Message); } CurrentSemanticValue.Messages = ValueStack[ValueStack.Depth-2].Messages; }
        break;
      case 5: // MessageBlock -> MessageComments, MessageContext, MessageId, MessageTranslations
{
			CurrentSemanticValue.Message = new GettextLib.Catalog.Translation();
			if (ValueStack[ValueStack.Depth-4].comments != null)	CurrentSemanticValue.Message.Comment = ValueStack[ValueStack.Depth-4].comments;
			if (ValueStack[ValueStack.Depth-3].MessageContext != null) CurrentSemanticValue.Message.MessageContext = ValueStack[ValueStack.Depth-3].MessageContext;
			
			CurrentSemanticValue.Message.MessageId = ValueStack[ValueStack.Depth-2].MessageId.MessageId;
			CurrentSemanticValue.Message.MessageIdPlural = ValueStack[ValueStack.Depth-2].MessageId.MessageIdPlural;
			
			CurrentSemanticValue.Message.MessageTranslations = ValueStack[ValueStack.Depth-1].MessageTranslations;
		}
        break;
      case 8: // MessageComments -> Comments
{ CurrentSemanticValue.comments = ValueStack[ValueStack.Depth-1].comments; }
        break;
      case 9: // MessageComments -> /* empty */
{ CurrentSemanticValue.comments = null; }
        break;
      case 10: // MessageId -> MSGID, MultiLineString
{
			CurrentSemanticValue.MessageId = new GettextLib.Catalog.Translation();
			CurrentSemanticValue.MessageId.MessageId = ValueStack[ValueStack.Depth-1].multiLineString;
		}
        break;
      case 11: // MessageId -> MSGID, MultiLineString, MSGID_PLURAL, MultiLineString
{
			CurrentSemanticValue.MessageId = new GettextLib.Catalog.Translation();
			CurrentSemanticValue.MessageId.MessageId = ValueStack[ValueStack.Depth-3].multiLineString;
			CurrentSemanticValue.MessageId.MessageIdPlural = ValueStack[ValueStack.Depth-1].multiLineString;
		}
        break;
      case 12: // MessageContext -> MSGCTXT, MultiLineString
{ CurrentSemanticValue.MessageContext = ValueStack[ValueStack.Depth-1].multiLineString; }
        break;
      case 13: // MessageContext -> /* empty */
{ CurrentSemanticValue.MessageContext = null; }
        break;
      case 14: // MessageTranslations -> MessageTranslation
{ CurrentSemanticValue.MessageTranslations = new List<GettextLib.Catalog.Translation.TranslationString>(); CurrentSemanticValue.MessageTranslations.Add(ValueStack[ValueStack.Depth-1].MessageTranslation); }
        break;
      case 15: // MessageTranslations -> MessageTranslations, MessageTranslation
{ ValueStack[ValueStack.Depth-2].MessageTranslations.Add(ValueStack[ValueStack.Depth-1].MessageTranslation); CurrentSemanticValue.MessageTranslations = ValueStack[ValueStack.Depth-2].MessageTranslations; }
        break;
      case 16: // MessageTranslation -> MSGSTR, MultiLineString
{
			CurrentSemanticValue.MessageTranslation = new GettextLib.Catalog.Translation.TranslationString();
			CurrentSemanticValue.MessageTranslation.Message = ValueStack[ValueStack.Depth-1].multiLineString;
			CurrentSemanticValue.MessageTranslation.Index = 0;
		}
        break;
      case 17: // MessageTranslation -> MSGSTR, LBRACKET, DIGIT, RBRACKET, MultiLineString
{
			CurrentSemanticValue.MessageTranslation = new GettextLib.Catalog.Translation.TranslationString();
			CurrentSemanticValue.MessageTranslation.Message = ValueStack[ValueStack.Depth-1].multiLineString;
			CurrentSemanticValue.MessageTranslation.Index = ValueStack[ValueStack.Depth-3].Int;
		}
        break;
      case 18: // Comments -> COMMENT
{ CurrentSemanticValue.comments = new GettextLib.Catalog.MultiLineString(); CurrentSemanticValue.comments.AddLine(ValueStack[ValueStack.Depth-1].String); }
        break;
      case 19: // Comments -> Comments, COMMENT
{ ValueStack[ValueStack.Depth-2].comments.AddLine(ValueStack[ValueStack.Depth-1].String); CurrentSemanticValue.comments = ValueStack[ValueStack.Depth-2].comments; }
        break;
      case 20: // MultiLineString -> STRING, EOL
{
				CurrentSemanticValue.multiLineString = new GettextLib.Catalog.MultiLineString();
				CurrentSemanticValue.multiLineString.AddLine(ValueStack[ValueStack.Depth-2].String);
		     }
        break;
      case 21: // MultiLineString -> STRING, EOF
{
				CurrentSemanticValue.multiLineString = new GettextLib.Catalog.MultiLineString();
				CurrentSemanticValue.multiLineString.AddLine(ValueStack[ValueStack.Depth-2].String);	
	             }
        break;
      case 22: // MultiLineString -> MultiLineString, STRING, EOL
{
						ValueStack[ValueStack.Depth-3].multiLineString.AddLine(ValueStack[ValueStack.Depth-2].String);
						CurrentSemanticValue.multiLineString = ValueStack[ValueStack.Depth-3].multiLineString;
					}
        break;
      case 23: // MultiLineString -> MultiLineString, STRING, EOF
{
						ValueStack[ValueStack.Depth-3].multiLineString.AddLine(ValueStack[ValueStack.Depth-2].String);
						CurrentSemanticValue.multiLineString = ValueStack[ValueStack.Depth-3].multiLineString;
					}
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }


public GettextLib.Catalog.GettextCatalog Catalog { get; private set; }

public Parser(Scanner scn) : base(scn) { }


}
}
