using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextMvcLib
{
    public static class Utils
    {
        public static string PseudoTranslate(string str)
        {
            return string.Format("[!{0}!]", str);
        }
    }
}
