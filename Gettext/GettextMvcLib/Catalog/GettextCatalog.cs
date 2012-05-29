using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextMvcLib.Catalog
{
    public class GettextCatalog
    {
        public List<Translation> Translations { get; set; }

        public GettextCatalog()
        {
            Translations = new List<Translation>();
        }

        public void AddTranslation(Translation translation)
        {
            if (translation == null) throw new ArgumentNullException("translation");
            Translations.Add(translation);
        }

        public void AddTranslations(IEnumerable<Translation> translations)
        {
            if (translations == null) return;

            foreach (var translation in translations)
            {
                AddTranslation(translation);
            }
        }
    }
    
    public class Translation
    {
        public MultiLineString MessageId { get; set; }
        public MultiLineString MessageIdPlural { get; set; }
        public MultiLineString MessageContext { get; set; }
        public List<TranslationString> MessageTranslations { get; set; }
        public MultiLineString Comment { get; set; }

        public Translation()
        {
            MessageId = new MultiLineString();
            MessageIdPlural = new MultiLineString();
            MessageTranslations = new List<TranslationString>();
            MessageContext = new MultiLineString();
            Comment = new MultiLineString();
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

        public string String { get { return string.Join("", Lines ?? new List<string>()); } }
    }
}
