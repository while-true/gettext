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
        public static string _(string msgid)
        {
            return GettextMvcUtils.GetGettext()._(msgid);
        }

        public static string _(string msgid, string msgidPlural, long n)
        {
            return GettextMvcUtils.GetGettext().NGettext(msgid, msgidPlural, n);
        }

        public static string PGettext(string msgctxt, string msgid)
        {
            return GettextMvcUtils.GetGettext().PGettext(msgctxt, msgid);
        }

        public static string PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            return GettextMvcUtils.GetGettext().PNGettext(msgctxt, msgid, msgidPlural, n);
        }
    }
}
