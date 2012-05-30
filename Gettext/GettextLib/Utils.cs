using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GettextLib
{
    public static class Utils
    {
        public static string PseudoTranslate(string str)
        {
            return string.Format("[!{0}!]", str);
        }
    }
}
