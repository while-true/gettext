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
            if (helper == null || helper.ViewData == null) return GettextUtils.CreateNullContext();
            var gc = helper.ViewData[GettextConsts.GettextContextKey] as GettextTranslationContext;
            if (gc == null) return GettextUtils.CreateNullContext();

            return gc;
        }

        public static GettextMvcTranslatedString _(this HtmlHelper helper, string msgid)
        {
            var ctx = GetContext(helper);
            return new GettextMvcTranslatedString(ctx.Gettext._(msgid), ctx.Culture);
        }

        public static GettextMvcTranslatedString _(this HtmlHelper helper, string msgid, string msgidPlural, int n)
        {
            var ctx = GetContext(helper);
            return new GettextMvcTranslatedString(ctx.Gettext.NGettext(msgid, msgidPlural, n), ctx.Culture);
        }
    }
}
