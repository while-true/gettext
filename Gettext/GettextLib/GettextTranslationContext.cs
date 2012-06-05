using System;
using System.Globalization;

namespace GettextLib
{
    public class GettextTranslationContext
    {
        private readonly LanguageTranslation languageTranslation;

        internal GettextTranslationContext(LanguageTranslation languageTranslation)
        {
            if (languageTranslation == null) throw new ArgumentNullException("languageTranslation");
            this.languageTranslation = languageTranslation;
        }

        public CultureInfo Culture { get { return languageTranslation.Culture; } }
        public IGettext Gettext { get { return languageTranslation.Gettext; } }
    }
}
