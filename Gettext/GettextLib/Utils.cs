using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GettextLib
{
    public static class Utils
    {
        public static string PseudoTranslate(string str)
        {
            return string.Format("[!{0}!]", str);
        }
        

        public static GettextTranslationContext CreateNullContext()
        {
            return new GettextTranslationContext(new LanguageTranslation
            {
                Culture = CultureInfo.InvariantCulture,
                Gettext = new GettextDummy(),
                LangId = Consts.GettextNullLanguage
            });
        }
    }
}
