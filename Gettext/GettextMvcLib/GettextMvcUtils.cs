using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib;

namespace GettextMvcLib
{
    internal static class GettextMvcUtils
    {
        public static IGettext GetGettext()
        {
            if (System.Web.HttpContext.Current == null) return new GettextDummy();
            var gc = System.Web.HttpContext.Current.Items[Consts.GettextContextKey] as GettextTranslationContext;
            if (gc == null) return new GettextDummy();

            return gc.Gettext;
        }
    }
}
