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
        
        public static GettextTranslatedString _(this HtmlHelper helper, string msgid)
        {
            var ctx = GetContext(helper);
            return new GettextTranslatedString(ctx.Gettext._(msgid), ctx.Culture);
        }

        public static GettextTranslatedString _(this HtmlHelper helper, string msgid, string msgidPlural, int n)
        {
            var ctx = GetContext(helper);
            return new GettextTranslatedString(ctx.Gettext.NGettext(msgid, msgidPlural, n), ctx.Culture);
        }
    }
}
