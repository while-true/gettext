using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GettextLib.Catalog;

namespace GettextLib
{
    public class GettextFactory : GettextFactoryBase
    {
        public GettextFactory()
        {
            catalogs = new List<LanguageTranslation>();
        }

        private List<LanguageTranslation> catalogs;
        
        /// <summary>
        /// Adds translations to the factory.
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="poFileContent"></param>
        /// <param name="culture">Culture to use for this translations. If it isn't specified, use the invariant culture.</param>
        public void AddTranslation(string languageId, Stream poFileContent, CultureInfo culture = null)
        {
            var catalog = GettextCatalog.ParseFromStream(poFileContent);

            culture = culture ?? CultureInfo.InvariantCulture;

            catalogs.Add(new LanguageTranslation(languageId, new Gettext(catalog), culture));
        }

        public override GettextTranslationContext GetContext(string langId)
        {
            if (string.IsNullOrWhiteSpace(langId) || langId == GettextConsts.GettextNullLanguage)
            {
                return GetNullContext();
            }

            if (langId == GettextConsts.GettextPseudoLanguage)
            {
                return GetPseudoContext();
            }

            var l = catalogs.SingleOrDefault(x => x.LangId == langId);
            if (l == null)
            {
                return GetNullContext();
            }

            return new GettextTranslationContext(l);
        }
    }
}
