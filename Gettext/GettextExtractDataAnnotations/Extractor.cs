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

            sb.Append(@"# SOME DESCRIPTIVE TITLE.
# Copyright (C) YEAR THE PACKAGE'S COPYRIGHT HOLDER
# This file is distributed under the same license as the PACKAGE package.
# FIRST AUTHOR <EMAIL@ADDRESS>, YEAR.
#
#, fuzzy
msgid """"
msgstr """"
""Project-Id-Version: PACKAGE VERSION\n""
""Report-Msgid-Bugs-To: \n""
""POT-Creation-Date: 2012-05-30 12:08+0200\n""
""PO-Revision-Date: YEAR-MO-DA HO:MI+ZONE\n""
""Last-Translator: FULL NAME <EMAIL@ADDRESS>\n""
""Language-Team: LANGUAGE <LL@li.org>\n""
""Language: \n""
""MIME-Version: 1.0\n""
""Content-Type: text/plain; charset=CHARSET\n""
""Content-Transfer-Encoding: 8bit\n""");
            sb.Append("\n\n");

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

            var p = translations.SingleOrDefault(x => x.Str == str);
            if (p != null) return;

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
