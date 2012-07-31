using System.Globalization;

namespace GettextLib
{
    public interface IGettextTranslationContext
    {
        CultureInfo Culture { get; }
        IGettext Gettext { get; }
        GettextTranslatedString _(string msgid);
        GettextTranslatedString _(string msgid, string msgidplural, long n);
        GettextTranslatedString PGettext(string msgctxt, string msgid);
        GettextTranslatedString PNGettext(string msgctxt, string msgid, string msgidPlural, long n);
    }
}