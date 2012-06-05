using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib.Catalog;

namespace GettextLib
{
    public class Gettext : IGettext
    {
        public GettextCatalog Catalog { get; private set; }

        public Gettext(GettextCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException("catalog");
            Catalog = catalog;
        }

        private Translation.TranslationString Lookup(string context, string msgid)
        {
            context = context ?? string.Empty;
            var translation = Catalog.Lookup(context, msgid, null);
            if (translation == null) return null;

            var t = translation.MessageTranslations.FirstOrDefault(x => x.Index == 0);
            return t;
        }

        private Translation LookupPlural(string context, string msgid, string msgidplural)
        {
            context = context ?? string.Empty;
            var translation = Catalog.Lookup(context, msgid, msgidplural);
            if (translation == null) return null;

            if (translation.MessageTranslations.Count != Catalog.NPlurals) return null;
            return translation;
        }

        public string _(string msgid)
        {
            var l = Lookup(null, msgid);
            if (l == null) return msgid;
            return l.Message.String;
        }

        public string NGettext(string msgid, string msgidPlural, long n)
        {
            var pl = LookupPlural(null, msgid, msgidPlural);
            return PluralTranslate(msgid, msgidPlural, n, pl);
        }

        private string PluralTranslate(string msgid, string msgidPlural, long n, Translation pl)
        {
            if (pl == null) goto fallback;

            var idx = Catalog.GetPluralIndex(n);
            var t = pl.MessageTranslations.SingleOrDefault(x => x.Index == idx);
            if (t == null) goto fallback;

            return t.Message.String;

            fallback:
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
            var pl = LookupPlural(msgctxt, msgid, msgidPlural);
            return PluralTranslate(msgid, msgidPlural, n, pl);
        }
    }
}
