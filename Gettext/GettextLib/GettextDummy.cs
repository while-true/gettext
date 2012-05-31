namespace GettextLib
{
    public class GettextDummy : IGettext
    {
        public virtual string _(string msgid)
        {
            return msgid;
        }

        public string PGettext(string msgctxt, string msgid)
        {
            throw new System.NotImplementedException();
        }

        public string PNGettext(string msgctxt, string msgid, string msgidPlural, long n)
        {
            throw new System.NotImplementedException();
        }

        public virtual string NGettext(string msgid, string msgidPlural, long n)
        {
            return n == 1 ? msgid : msgidPlural;
        }
    }
}