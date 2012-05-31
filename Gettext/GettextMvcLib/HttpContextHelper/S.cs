using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib;

namespace GettextMvcLib.HttpContextHelper
{
    public static class S
    {
        private static IGettext GetGettext()
        {
            if (System.Web.HttpContext.Current == null) return new GettextDummy();
            var gc = System.Web.HttpContext.Current.Items[Consts.GettextContextKey] as GettextTranslationContext;
            if (gc == null) return new GettextDummy();

            return gc.Gettext;
        }

        public static string _(string msgid)
        {
            return GetGettext()._(msgid);
        }

        public static string _(string msgid, string msgidPlural, long n)
        {
            return GetGettext().NGettext(msgid, msgidPlural, n);
        }

        public static string PGettext(string msgctxt, string msgid)
        {
            return GetGettext().PGettext(msgctxt, msgid);
        }

        public static string PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            return GetGettext().PNGettext(msgctxt, msgid, msgidPlural, n);
        }
    }
}
