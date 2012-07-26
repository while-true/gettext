using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib
{
    public interface IGettext
    {
        string _(string msgid);
        string NGettext(string msgid, string msgidPlural, long n);
        string PGettext(string msgctxt, string msgid);
        string PNGettext(string msgctxt, string msgid, string msgidPlural, long n);
    }
}
