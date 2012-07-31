using System;
using System.Globalization;

namespace GettextLib
{
    public class GettextTranslationContext : IGettextTranslationContext
    {
        private readonly LanguageTranslation languageTranslation;

        internal GettextTranslationContext(LanguageTranslation languageTranslation)
        {
            if (languageTranslation == null) throw new ArgumentNullException("languageTranslation");
            this.languageTranslation = languageTranslation;
        }

        public CultureInfo Culture { get { return languageTranslation.Culture; } }
        public IGettext Gettext { get { return languageTranslation.Gettext; } }

        public GettextTranslatedString _(string msgid)
        {
            return new GettextTranslatedString(Gettext._(msgid), Culture);
        }

        public GettextTranslatedString _(string msgid, string msgidplural, long n)
        {
            return new GettextTranslatedString(Gettext.NGettext(msgid, msgidplural, n), Culture);
        }

        public GettextTranslatedString PGettext(string msgctxt, string msgid)
        {
            return new GettextTranslatedString(Gettext.PGettext(msgctxt, msgid), Culture);
        }

        public GettextTranslatedString PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            return new GettextTranslatedString(Gettext.PNGettext(msgctxt, msgid, msgidPlural, n), Culture);
        }
    }
}
