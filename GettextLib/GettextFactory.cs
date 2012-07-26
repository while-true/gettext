using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GettextLib.Catalog;

namespace GettextLib
{
    public class GettextFactory
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
        public void AddTranslation(string languageId, string poFileContent, CultureInfo culture = null)
        {
            var catalog = GettextCatalog.ParseFromPoString(poFileContent);

            culture = culture ?? CultureInfo.InvariantCulture;

            catalogs.Add(new LanguageTranslation
                             {
                                 Gettext = new Gettext(catalog),
                                 LangId = languageId,
                                 Culture = culture
                             });
        }

        public GettextTranslationContext GetContext(string langId)
        {
            if (langId == GettextConsts.GettextPseudoLanguage)
            {
                return new GettextTranslationContext(new LanguageTranslation
                                                         {
                                                             LangId = GettextConsts.GettextPseudoLanguage,
                                                             Gettext = new GettextPseudo()
                                                         });
            }

			if (langId == GettextConsts.GettextDefaultLanguage)
			{
				return GetNullContext();
			}
			
            var l = catalogs.SingleOrDefault(x => x.LangId == langId);
            if (l == null)
            {
                //throw new Exception("Language not found");
                return GetNullContext();
            }

            return new GettextTranslationContext(l);
        }

        /// <summary>
        /// Return pseudoized strings.
        /// </summary>
        /// <returns></returns>
        public GettextTranslationContext GetPseudoContext()
        {
            return GetContext(GettextConsts.GettextPseudoLanguage);
        }

        /// <summary>
        /// No translations - return what we get.
        /// </summary>
        /// <returns></returns>
        public GettextTranslationContext GetNullContext()
        {
            return new GettextTranslationContext(new LanguageTranslation
                                                     {
                                                         LangId = "null",
                                                         Gettext = new GettextDummy()
                                                     });
        }
    }
}
