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

        public string ToHtmlString()
        {
            return String;
        }
    }
}
