using System.Globalization;

namespace GettextLib
{
    public abstract class GettextFactoryBase : IGettextFactory
    {
        public abstract GettextTranslationContext GetContext(string langId);

        /// <summary>
        /// Return pseudoized strings.
        /// </summary>
        /// <returns></returns>
        public GettextTranslationContext GetPseudoContext()
        {
            return new GettextTranslationContext(new LanguageTranslation(GettextConsts.GettextPseudoLanguage, new GettextPseudo(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// No translations - return what we get.
        /// </summary>
        /// <returns></returns>
        public GettextTranslationContext GetNullContext()
        {
            return new GettextTranslationContext(new LanguageTranslation(GettextConsts.GettextNullLanguage, new GettextDummy(), CultureInfo.InvariantCulture));
        }
    }
}