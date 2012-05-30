namespace GettextLib
{
    public class GettextDummy : IGettext
    {
        public virtual string _(string msgid)
        {
            return msgid;
        }

        public virtual string NGettext(string msgid, string msgidPlural, int n)
        {
            return n == 1 ? msgid : msgidPlural;
        }
    }
}