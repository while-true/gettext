using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib
{
    public interface IGettext
    {
        string _(string msgid);
        string NGettext(string msgid, string msgidPlural, int n);
    }
}
