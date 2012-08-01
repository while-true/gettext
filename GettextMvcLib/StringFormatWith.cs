using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using GettextLib;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace GettextMvcLib
{
    public static class StringFormatWith
    {
        public static MvcHtmlString ToHtmlString([NotNull] this GettextTranslatedString str)
        {
            if (str == null) throw new ArgumentNullException("str");
            
            return MvcHtmlString.Create(str);
        }

        /// <summary>
        /// Calls String.Format using the supplied arguements 
        /// to return an Html encoded string
        /// </summary>
        /// <param name="format">The object being formatted</param>
        /// <param name="htmlEncode">Passing any bool indicates the object should html encoded</param>
        /// <param name="args">objects that may not be strings that will replace formatting characters</param>
        /// <returns>an HtmlString with all formatting characters replaced by the passed in arguements</returns>
        public static MvcHtmlString HtmlFormat(this GettextTranslatedString format, params object[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            return MvcHtmlString.Create(string.Format(format.CultureInfo, format, args));
        }

        public static MvcHtmlString HtmlFormatWith([NotNull] this GettextTranslatedString format, object obj)
        {
            if (format == null) throw new ArgumentNullException("format");

            return MvcHtmlString.Create(format.FormatWith(obj));
        }
    }
}
