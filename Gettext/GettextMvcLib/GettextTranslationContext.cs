using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib;

namespace GettextMvcLib
{
    public class GettextTranslationContext
    {
        private readonly LanguageTranslation languageTranslation;

        internal GettextTranslationContext(LanguageTranslation languageTranslation)
        {
            if (languageTranslation == null) throw new ArgumentNullException("languageTranslation");
            this.languageTranslation = languageTranslation;
        }

        public IGettext Gettext { get { return languageTranslation.Gettext; } }
    }
}
