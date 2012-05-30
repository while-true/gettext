using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GettextLib;
using Gettext = GettextLib.Gettext;

namespace GettextMvcLib.MvcHtmlHelper
{
    public static class GettextExtensions
    {
        private static IGettext GetGettext(HtmlHelper helper)
        {
            if (helper == null || helper.ViewData == null) return new GettextDummy();
            var gc = helper.ViewData[Consts.GettextContextKey] as GettextTranslationContext;
            if (gc == null) return new GettextDummy();

            return gc.Gettext;
        }

        public static string _(this HtmlHelper helper, string str)
        {
            return GetGettext(helper)._(str);
        }

        public static string _(this HtmlHelper helper, string msgid, string msgidPlural, int n)
        {
            return GetGettext(helper).NGettext(msgid, msgidPlural, n);
        }
    }
}
