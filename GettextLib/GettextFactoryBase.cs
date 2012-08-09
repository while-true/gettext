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
            return new GettextTranslationContext(new LanguageTranslation
                {
                    LangId = GettextConsts.GettextPseudoLanguage,
                    Gettext = new GettextPseudo()
                });
        }

        /// <summary>
        /// No translations - return what we get.
        /// </summary>
        /// <returns></returns>
        public GettextTranslationContext GetNullContext()
        {
            return new GettextTranslationContext(new LanguageTranslation
                {
                    LangId = GettextConsts.GettextNullLanguage,
                    Gettext = new GettextDummy()
                });
        }
    }
}