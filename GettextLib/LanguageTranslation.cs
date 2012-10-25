using System;
using System.Globalization;

namespace GettextLib
{
    internal class LanguageTranslation
    {
        public LanguageTranslation(string langId, IGettext gettext, CultureInfo culture)
        {
            if (gettext == null) throw new ArgumentNullException("gettext");
            if (culture == null) throw new ArgumentNullException("culture");
            LangId = langId;
            Gettext = gettext;
            Culture = culture;
        }

        public string LangId { get; private set; }
        public IGettext Gettext { get; private set; }
        public CultureInfo Culture { get; private set; }
    }
}