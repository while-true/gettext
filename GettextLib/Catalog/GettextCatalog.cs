using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GettextLib.ExpressionEvaluator;
using JetBrains.Annotations;
using Scanner = GettextLib.Parser.Scanner;

namespace GettextLib.Catalog
{
    public class GettextCatalog
    {
        public int NPlurals { get; set; }
        public Func<long, int> GetPluralIndex { get; set; }
        internal List<Translation> Translations { get; set; }
        public Dictionary<string, string> Headers { get; private set; }

        internal Dictionary<string, Translation> TranslationLookup { get; private set; }

        public const string PluralFormsHeaderKey = "Plural-Forms";

        internal bool TranslateFuzzyTranslations { get; private set; }

        internal GettextCatalog()
        {
            Translations = new List<Translation>();
            NPlurals = 2;
            GetPluralIndex = n => n == 1 ? 0 : 1;
            Headers = new Dictionary<string, string>();
            TranslateFuzzyTranslations = false;
        }

        public const string ContextSeparator = "\u0004";

        private static string LookupKey(string messageContext, string messageId, string messageIdPlural)
        {
            var parts = new[] { messageContext, messageId, messageIdPlural }.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (parts.Length == 0) return null;
            return string.Join(ContextSeparator, parts);
        }

        internal Translation Lookup(string messageContext, string messageId, string messageIdPlural)
        {
            var key = LookupKey(messageContext, messageId, messageIdPlural);

            if (string.IsNullOrWhiteSpace(key)) return null;

            Translation trans;
            if (TranslationLookup.TryGetValue(key, out trans))
            {
                return trans;
            }

            return null;
        }

        /// <summary>
        /// Builds lookup structures
        /// </summary>
        internal void Finalize()
        {
            TranslationLookup = new Dictionary<string, Translation>();

            foreach (var translation in Translations)
            {
                if (translation.Fuzzy && !TranslateFuzzyTranslations) continue;

                var key = LookupKey(translation.MessageContext.String, translation.MessageId.String, translation.MessageIdPlural.String);

                if (!string.IsNullOrWhiteSpace(key) && !TranslationLookup.ContainsKey(key))
                {
                    TranslationLookup.Add(key, translation);
                }
            }
        }

        internal void AddTranslation(Translation translation)
        {
            if (translation == null) throw new ArgumentNullException("translation");
            Translations.Add(translation);
        }

        internal void AddTranslations(IEnumerable<Translation> translations)
        {
            if (translations == null) return;

            foreach (var translation in translations)
            {
                AddTranslation(translation);
            }
        }

        public static GettextCatalog ParseFromStream([NotNull] Stream poStream)
        {
            if (poStream == null) throw new ArgumentNullException("poStream");

            GettextCatalog catalog = null;
            try
            {
                
                var lexer = new Scanner();
                lexer.SetSource(poStream);

                var parser = new Parser.Parser(lexer);
                parser.Parse();

                catalog = parser.Catalog;

                if (catalog == null) goto ret;

                // another parsing step
                catalog.ParseHeaders();

                // transform all strings into internal UTF-8 representation
                catalog.ConvertStringsToUtf8();

                // parse comments
                catalog.ParseComments();

                // build lookup structures
                catalog.Finalize();

            } catch (Exception e)
            {
                throw new GettextException("Parsing exception!", e);
            }

            ret:

            if (catalog == null) throw new GettextException("Couldn't parse the catalog. Check the syntax.");
            return catalog;
        }

        private void ConvertStringsToUtf8()
        {
            var sourceCodePage = "";
            {
                if (Headers["Content-Type"] != null)
                {
                    var ct = (Headers["Content-Type"] ?? "").Trim();

                    var r = new Regex("(.*?)charset=(.*)");
                    var m = r.Match(ct);
                    if (m.Success)
                    {
                        sourceCodePage = m.Groups[2].Value;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(sourceCodePage))
            {
                var enc = Encoding.GetEncoding(sourceCodePage);
                
                foreach (var translation in Translations)
                {
                    Convert(translation.Comment, enc);
                    Convert(translation.MessageContext, enc);
                    Convert(translation.MessageId, enc);
                    Convert(translation.MessageIdPlural, enc);

                    foreach (var t in translation.MessageTranslations)
                    {
                        Convert(t.Message, enc);
                    }
                }

                foreach (var k in Headers.Keys.ToList())
                {
                    Headers[k] = Convert(Headers[k], enc);
                }
            }
        }

        private static void Convert(MultiLineString str, Encoding enc)
        {
            str.Lines = str.Lines.Select(s => Convert(s, enc)).ToList();
        }

        private static string Convert(String str, Encoding enc)
        {
            var b = str.Select(c => (byte)c).ToArray();
            return enc.GetString(b);
        }

        private void ParseComments()
        {
            foreach (var translation in Translations)
            {
                if (translation.Comment.Lines != null)
                {
                    foreach (var t in translation.Comment.Lines)
                    {
                        if (t.StartsWith(","))
                        {
                            if (t.Contains("fuzzy"))
                            {
                                translation.Fuzzy = true;
                            }
                        }
                    }
                }
            }
        }

        private void ParseHeaders()
        {
            var header = Translations.SingleOrDefault(x => x.MessageId.String == "");
            if (header == null) return;

            var t = header.MessageTranslations.SingleOrDefault();
            if (t == null) return;

            var st = t.Message.String;
            if (string.IsNullOrWhiteSpace(st)) return;

            var lines = st.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var r = new Regex(@"([\w-]+): (.*)", RegexOptions.None);
                var m = r.Match(line);
                if (m.Success)
                {
                    var key = m.Groups[1].Value;
                    var value = m.Groups[2].Value;

                    if (!Headers.ContainsKey(key))
                    {
                        Headers.Add(key, value);
                    } else
                    {
                        // overwrite
                        Headers[key] = value;
                    }
                }
            }

            ParsePluralFormsHeader();
        }

        private void ParsePluralFormsHeader()
        {
            string pluralForms;
            if (!Headers.TryGetValue(PluralFormsHeaderKey, out pluralForms)) return;

            Script script = null;
            try
            {
                var scanner = new ExpressionEvaluator.Scanner();
                scanner.SetSource(pluralForms, 0);
                var parser = new ExpressionEvaluator.Parser(scanner);

                parser.Parse();

                script = parser.Script;
            } catch (Exception e)
            {
                throw new GettextException("Error parsing the plural forms header", e);
            }

            if (script == null) return;

            var assignmentNplurals = script.Assignments.SingleOrDefault(x => x.Var == "nplurals");
            var assignmentN = script.Assignments.SingleOrDefault(x => x.Var == "plural");
            if (assignmentNplurals == null || assignmentN == null) return;

            {
                var state = new ExpressionState();
                assignmentNplurals.Execute(state);
                NPlurals = (int) state.GetVar("nplurals");
            }

            {
                PluralExpression = assignmentN;

                GetPluralIndex = n =>
                                     {
                                         var state = new ExpressionState();
                                         state.SetVar("nplurals", NPlurals);
                                         state.SetVar("n", n);
                                         assignmentN.Execute(state);
                                         return (int) state.GetVar("plural");
                                     };
            }
        }

        internal Assignment PluralExpression { get; private set; }
    }
    
    public class Translation
    {
        public MultiLineString MessageId { get; set; }
        public MultiLineString MessageIdPlural { get; set; }
        public MultiLineString MessageContext { get; set; }
        public List<TranslationString> MessageTranslations { get; set; }
        public MultiLineString Comment { get; set; }
        public bool Fuzzy { get; set; }

        public Translation()
        {
            MessageId = new MultiLineString();
            MessageIdPlural = new MultiLineString();
            MessageTranslations = new List<TranslationString>();
            MessageContext = new MultiLineString();
            Comment = new MultiLineString();
            Fuzzy = false;
        }

        public class TranslationString
        {
            public MultiLineString Message { get; set; }
            public int Index { get; set; }
        }
    }

    public class MultiLineString
    {
        public List<string> Lines { get; set; } 

        public MultiLineString()
        {
            Lines = new List<string>();
        }

        public void AddLine(string line)
        {
            Lines.Add(line);
        }

        public string String { get
        {
            if (Lines == null || Lines.Count == 0) return string.Empty;
            return string.Join("", Lines);
        } }
    }
}
