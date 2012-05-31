namespace GettextLib
{
    public class GettextPseudo : GettextDummy
    {
        public override string _(string msgid)
        {
            return Utils.PseudoTranslate(base._(msgid));
        }

        public override string NGettext(string msgid, string msgidPlural, long n)
        {
            return Utils.PseudoTranslate(base.NGettext(msgid, msgidPlural, n));
        }
    }
}