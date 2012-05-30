using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GettextExtractDataAnnotations
{
    public class Extractor
    {
        public Extractor()
        {
            translations = new List<TranslationString>();
        }

        public void Parse(Assembly assembly)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                Console.WriteLine("Type {0}", type.Name);

                Parse(type);

                var props = type.GetProperties();
                foreach (var propertyInfo in props)
                {
                    Parse(propertyInfo);
                }

                
            }
        }

        public string ToPoString()
        {
            var sb = new StringBuilder();

            foreach (var translationString in translations)
            {
                sb.AppendFormat("msgid \"{0}\"\n", Escape(translationString.Str));
                sb.AppendFormat("msgstr \"\"\n");
                sb.Append("\n");
            }

            return sb.ToString();
        }

        private static string Escape(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            return str.Replace("\"", "\\\"");
        }

        private List<TranslationString> translations; 

        private class TranslationString
        {
            public string Str { get; set; }

            public TranslationString(string str)
            {
                Str = str;
            }
        }

        private void AddTranslation(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return;
            translations.Add(new TranslationString(str));
        }

        private void Parse(MemberInfo info)
        {
            var d = info.GetCustomAttributes(true);
            if (d.Length == 0) return;

            foreach (var attribute in d)
            {
                var displayAttribute = attribute as DisplayAttribute;
                if (displayAttribute != null)
                {
                    AddTranslation(displayAttribute.Name);
                }

                var va = attribute as ValidationAttribute;
                if (va != null)
                {
                    AddTranslation(va.ErrorMessage);
                }
            }
        }
    }
}
