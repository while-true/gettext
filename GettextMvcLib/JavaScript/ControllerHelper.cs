using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GettextLib;

namespace GettextMvcLib.JavaScript
{
    public static class ControllerHelper
    {
        public static ActionResult HandleRequest(Gettext gettext = null)
        {
            string js;

            using (var jsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GettextMvcLib.JavaScript.gettext.js"))
            using (var sr = new StreamReader(jsStream))
            {
                js = sr.ReadToEnd();
            }

            if (gettext == null)
            {
                var g = GettextMvcUtils.GetGettext();
                gettext = g as Gettext;
            }
            

            var jsonTr = "";

            if (gettext != null)
            {
                var jsTranslations = gettext.Catalog.Translations.Where(x =>
                                                                       {
                                                                           var c = x.Comment.String;
                                                                           if (string.IsNullOrWhiteSpace(c)) return false;
                                                                           return c.Contains(".js");
                                                                       }).ToList();

                var j = new JsonTranslations();

                if (jsTranslations.Count > 0)
                {
                    foreach (var jsTranslation in jsTranslations)
                    {
                        var t = new JsonTranslations.Translation();
                        t.P = jsTranslation.MessageIdPlural.String;

                        t.T = jsTranslation.MessageTranslations.OrderBy(x => x.Index).Select(x => x.Message.String).ToArray();

                        j.Translations.Add(jsTranslation.MessageId.String, t);
                    }
                }

                var ss = new JavaScriptSerializer();
                jsonTr = ss.Serialize(j);

                if (!string.IsNullOrWhiteSpace(jsonTr))
                {
                    string jsRepl = "__gettext_translations = " + jsonTr + ";\n";

                    if (gettext.Catalog.PluralExpression != null)
                    {
                        jsRepl += string.Format("__gettext_plural_func = function(n) {{ return {0} ; }};\n", gettext.Catalog.PluralExpression.ToPrint());
                    }



                    js = js.Replace("/* == TRANSLATION == */", jsRepl);
                }
            }
            
            return new ContentResult
                       {
                           Content = js,
                           ContentEncoding = Encoding.UTF8,
                           ContentType = "text/javascript"
                       };
        }

        private class JsonTranslations
        {
            public JsonTranslations()
            {
                Translations = new Dictionary<string, Translation>();
            }

            public Dictionary<string, Translation> Translations { get; set; }

            public class Translation
            {
                /// <summary>
                /// Translations
                /// </summary>
                public string[] T { get; set; }

                /// <summary>
                /// Plural
                /// </summary>
                public string P { get; set; }
            }
        }

    }
}
