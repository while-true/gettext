using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GettextLib
{
    public class GettextTranslatedString
    {
        public string String { get; private set; }
        public CultureInfo CultureInfo { get; private set; }

        public GettextTranslatedString(string str, CultureInfo cultureInfo = null)
        {
            cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;

            String = str;
            CultureInfo = cultureInfo;
        }

        public static implicit operator string(GettextTranslatedString g)
        {
            if (g == null) return string.Empty;
            return g.String;
        }

        public override string ToString()
        {
            return String;
        }
    }
}
