using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace GettextMvcLib
{
    public class GettextMvcTranslatedString : GettextLib.GettextTranslatedString, IHtmlString
    {
        public GettextMvcTranslatedString(string str, CultureInfo cultureInfo) : base(str, cultureInfo)
        {
        }

        public GettextMvcTranslatedString(GettextLib.GettextTranslatedString s) : base(s.String, s.CultureInfo)
        {
            if (s == null) throw new ArgumentNullException("s");
        }

        public string ToHtmlString()
        {
            return String;
        }
    }
}
