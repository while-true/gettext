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

        public static string _(string msgid, string msgidPlural, int n)
        {
            return GetGettext().NGettext(msgid, msgidPlural, n);
        }
    }
}
