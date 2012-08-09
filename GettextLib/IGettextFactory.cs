namespace GettextLib
{
    public interface IGettextFactory
    {
        GettextTranslationContext GetContext(string langId);

        /// <summary>
        /// Return pseudoized strings.
        /// </summary>
        /// <returns></returns>
        GettextTranslationContext GetPseudoContext();

        /// <summary>
        /// No translations - return what we get.
        /// </summary>
        /// <returns></returns>
        GettextTranslationContext GetNullContext();
    }
}