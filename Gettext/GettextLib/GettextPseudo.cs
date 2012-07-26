namespace GettextLib
{
    public class GettextPseudo : GettextDummy
    {
        public override string _(string msgid)
        {
            return GettextUtils.PseudoTranslate(base._(msgid));
        }

        public override string NGettext(string msgid, string msgidPlural, long n)
        {
            return GettextUtils.PseudoTranslate(base.NGettext(msgid, msgidPlural, n));
        }

        public override string PGettext(string msgctxt, string msgid)
        {
            return GettextUtils.PseudoTranslate(base.PGettext(msgctxt, msgid));
        }

        public override string PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            return GettextUtils.PseudoTranslate(base.PNGettext(msgctxt, msgid, msgidPlural, n));
        }
    }
}