using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib.Catalog;

namespace GettextLib
{
    public class Gettext : IGettext
    {
        private readonly GettextCatalog catalog;

        public Gettext(GettextCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException("catalog");
            this.catalog = catalog;
        }

        public string _(string msgid)
        {
            // find the original string
            var translation = catalog.Translations.SingleOrDefault(x => x.MessageId.String == msgid);
            if (translation == null) return msgid;
            var t = translation.MessageTranslations.FirstOrDefault(x => x.Index == 0);
            if (t == null) return msgid;

            return t.Message.String;
        }

        public string NGettext(string msgid, string msgidPlural, int n)
        {
            throw new NotImplementedException();
        }
    }
}
