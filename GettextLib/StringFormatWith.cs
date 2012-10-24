using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GettextLib
{
    public static class StringFormatWith
    {
        /// <summary>
        /// Calls String.Format with the current translation string's locale.
        /// </summary>
        /// <param name="format">The string being formatted</param>
        /// <param name="args">strings that will replace formatting characters</param>
        /// <returns>a string with all formatting characters replaced by the passed in arguments</returns>
        public static string Format(this GettextTranslatedString format, params object[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException("format");

            return string.Format(format.CultureInfo, format, args);
        }

        /// <summary>
        /// Calls String.Format with the current translation string's locale.
        /// 
        /// Uses named parameters.
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="source">Data object</param>
        /// <returns>Formatter string</returns>
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
                  : StringEval.Eval(source, propertyGroup.Value));

                return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value + new string('}', endGroup.Captures.Count);
            });

            return string.Format(format.CultureInfo, rewrittenFormat, values.ToArray());            
        }

        public static class StringEval
        {
            public static object Eval(object obj, string expr)
            {
                if (string.IsNullOrWhiteSpace(expr)) throw new ArgumentNullException("expr");
                expr = expr.Trim();
                
                if (obj == null) return null;

                object ret = obj;

                foreach (var part in expr.Split('.').Select(x => x.Trim()))
                {
                    if (ret == null) return null;

                    var type = ret.GetType();
                    var p = type.GetProperty(part);
                    if (p != null)
                    {
                        ret = p.GetValue(ret, null);
                        continue;
                    }
                    
                    var f = type.GetField(part);
                    if (f != null)
                    {
                        ret = f.GetValue(ret);
                        continue;
                    }

                    return "{missing}";

                    //throw new Exception(string.Format("Property or field \"{0}\" not found!", part));
                }

                return ret;
            }
        }
    }
}
