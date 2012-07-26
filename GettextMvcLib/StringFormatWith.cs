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

namespace GettextMvcLib
{
    public static class StringFormatWith
    {
        public static string FormatWith(this GettextTranslatedString format, object source)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            var r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            var values = new List<object>();
            var rewrittenFormat = r.Replace(format.String, delegate(Match m)
            {
                Group startGroup = m.Groups["start"];
                Group propertyGroup = m.Groups["property"];
                Group formatGroup = m.Groups["format"];
                Group endGroup = m.Groups["end"];

                values.Add((propertyGroup.Value == "0")
                  ? source
                  : DataBinder.Eval(source, propertyGroup.Value));

                return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value + new string('}', endGroup.Captures.Count);
            });

            return string.Format(format.CultureInfo, rewrittenFormat, values.ToArray());
        }

        /// <summary>
        /// Calls String.Format using the supplied arguements 
        /// </summary>
        /// <param name="format">The object being formatted</param>
        /// <param name="args">strings that will replace formatting characters</param>
        /// <returns>a string with all formatting characters replaced by the passed in arguements</returns>
        public static string FormatWith(this GettextTranslatedString format, params string[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            return string.Format(format.CultureInfo, format, args);
        }
    
        /// <summary>
        /// Converts a regular string to an Html encoded string
        /// </summary>
        /// <param name="format">The object being formatted</param>
        /// <param name="htmlEncode">Passing any bool indicates the object should html encoded</param>
        /// <returns>an HtmlString version of the original string</returns>
        public static MvcHtmlString HtmlFormat(this GettextTranslatedString format)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            return MvcHtmlString.Create(format);
        }
        /// <summary>
        /// Calls String.Format using the supplied arguements 
        /// to return an Html encoded string
        /// </summary>
        /// <param name="format">The object being formatted</param>
        /// <param name="htmlEncode">Passing any bool indicates the object should html encoded</param>
        /// <param name="args">objects that may not be strings that will replace formatting characters</param>
        /// <returns>an HtmlString with all formatting characters replaced by the passed in arguements</returns>
        public static MvcHtmlString HtmlFormatWith(this GettextTranslatedString format, params string[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            return MvcHtmlString.Create(string.Format(format.CultureInfo, format, args));
        }
    }
}
