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

        private Translation.TranslationString Lookup(string context, string msgid)
        {
            context = context ?? string.Empty;
            var translation = catalog.Translations.SingleOrDefault(x => string.Equals(msgid, x.MessageId.String) && string.Equals(context, x.MessageContext.String));
            if (translation == null) return null;

            var t = translation.MessageTranslations.FirstOrDefault(x => x.Index == 0);
            return t;
        }

        public string _(string msgid)
        {
            var l = Lookup(null, msgid);
            if (l == null) return msgid;
            return l.Message.String;
        }

        public string NGettext(string msgid, string msgidPlural, long n)
        {
            // dummy 
            return n == 1 ? msgid : msgidPlural;
        }

        public string PGettext(string msgctxt, string msgid)
        {
            var l = Lookup(msgctxt, msgid);
            if (l == null) return msgid;
            return l.Message.String;
        }

        public string PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            throw new NotImplementedException();
        }
    }
}
