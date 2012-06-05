using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GettextLib;
using Gettext = GettextLib.Gettext;

namespace GettextMvcLib.MvcHtmlHelper
{
    /// <summary>
    /// These methods get the gettext context from the helper's ViewData.
    /// </summary>
    public static class GettextExtensions
    {
        private static GettextTranslationContext GetContext(HtmlHelper helper)
        {
            if (helper == null || helper.ViewData == null) return Utils.CreateNullContext();
            var gc = helper.ViewData[Consts.GettextContextKey] as GettextTranslationContext;
            if (gc == null) return Utils.CreateNullContext();

            return gc;
        }

        private static IGettext GetGettext(HtmlHelper helper)
        {
            return GetContext(helper).Gettext;
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
