using System.Globalization;

namespace GettextLib
{
    internal class LanguageTranslation
    {
        public string LangId { get; set; }
        public IGettext Gettext { get; set; }
        public CultureInfo Culture { get; set; }
    }
}