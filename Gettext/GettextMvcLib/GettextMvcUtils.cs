using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GettextLib;

namespace GettextMvcLib
{
    internal static class GettextMvcUtils
    {
        /// <summary>
        /// Get translation context from HttpContext's items.
        /// </summary>
        /// <returns></returns>
        public static GettextTranslationContext GetTranslationContext()
        {
            if (System.Web.HttpContext.Current == null) goto dummy;
            var gc = System.Web.HttpContext.Current.Items[Consts.GettextContextKey] as GettextTranslationContext;
            if (gc == null) goto dummy;

            return gc;

            dummy:

            return Utils.CreateNullContext();
        }

        /// <summary>
        /// Get translation context from HttpContext's items.
        /// </summary>
        /// <returns></returns>
        public static IGettext GetGettext()
        {
            return GetTranslationContext().Gettext;
        }
    }
}
