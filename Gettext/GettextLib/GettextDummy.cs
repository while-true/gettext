namespace GettextLib
{
    public class GettextDummy : IGettext
    {
        public virtual string _(string msgid)
        {
            return msgid;
        }

        public virtual string PGettext(string msgctxt, string msgid)
        {
            return msgid;
        }

        public virtual string PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            return n == 1 ? msgid : msgidPlural;
        }

        public virtual string NGettext(string msgid, string msgidPlural, long n)
        {
            return n == 1 ? msgid : msgidPlural;
        }
    }
}