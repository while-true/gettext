using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib
{
    public static class StringFormatWith
    {
        /// <summary>
        /// DEPRECATED ~~~~~~~~ use GettextMvcLib.FormatWith instead
        /// </summary>
        /// <param name="format">The string being formatted</param>
        /// <param name="args">strings that will replace formatting characters</param>
        /// <returns>a string with all formatting characters replaced by the passed in arguements</returns>
        public static string FormatWith(this GettextTranslatedString format, params object[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            return string.Format(format.CultureInfo, format, args);
        }
    
    }
}
