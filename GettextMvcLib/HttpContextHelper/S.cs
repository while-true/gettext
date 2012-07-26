using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib;

namespace GettextMvcLib.HttpContextHelper
{
    /// <summary>
    /// These methods retrieve the gettext context from the current HttpContext's items.
    /// </summary>
    public static class S
    {
        public static GettextTranslatedString _(string msgid)
        {
            var ctx = GettextMvcUtils.GetTranslationContext();
            return new GettextTranslatedString(ctx.Gettext._(msgid), ctx.Culture);
        }

        public static GettextTranslatedString _(string msgid, string msgidPlural, long n)
        {
            var ctx = GettextMvcUtils.GetTranslationContext();
            return new GettextTranslatedString(ctx.Gettext.NGettext(msgid, msgidPlural, n), ctx.Culture);
        }

        public static GettextTranslatedString PGettext(string msgctxt, string msgid)
        {
            var ctx = GettextMvcUtils.GetTranslationContext();
            return new GettextTranslatedString(ctx.Gettext.PGettext(msgctxt, msgid), ctx.Culture);
        }

        public static GettextTranslatedString PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            var ctx = GettextMvcUtils.GetTranslationContext();
            return new GettextTranslatedString(ctx.Gettext.PNGettext(msgctxt, msgid, msgidPlural, n), ctx.Culture);
        }
    }
}
