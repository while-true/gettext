using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GettextLib;
using GettextLib.Catalog;
using Gettext = GettextLib.Gettext;

namespace GettextMvcLib
{

    internal class LanguageTranslation
    {
        public string LangId { get; set; }
        public IGettext Gettext { get; set; }
    }

    public class GettextFactory
    {
        public GettextFactory()
        {
            catalogs = new List<LanguageTranslation>();
        }

        private List<LanguageTranslation> catalogs;
        
        public void AddTranslation(string language, string content)
        {
            var catalog = GettextCatalog.ParseFromPoString(content);
            
            catalogs.Add(new LanguageTranslation
                             {
                                 Gettext = new Gettext(catalog),
                                 LangId = language
                             });
        }

        public GettextTranslationContext GetContext(string langId)
        {
            if (langId == Consts.GettextPseudoLanguage)
            {
                return new GettextTranslationContext(new LanguageTranslation
                                                         {
                                                             LangId = Consts.GettextPseudoLanguage,
                                                             Gettext = new GettextPseudo()
                                                         });
            }

            var l = catalogs.SingleOrDefault(x => x.LangId == langId);
            if (l == null) throw new Exception("Language not found");

            return new GettextTranslationContext(l);
        }

        public GettextTranslationContext GetPseudoContext()
        {
            return GetContext(Consts.GettextPseudoLanguage);
        }

        public void SetAsContextForCurrentRequest(GettextTranslationContext ctx, ViewDataDictionary viewData)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");
            if (viewData == null) throw new ArgumentNullException("viewData");

            viewData.Add(Consts.GettextContextKey, ctx);
            System.Web.HttpContext.Current.Items[Consts.GettextContextKey] = ctx;
        }
    }
}
