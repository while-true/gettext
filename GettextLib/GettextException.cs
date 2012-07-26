using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib
{
    public class GettextException : Exception
    {
        internal GettextException(string message) : base(message)
        {
            
        }

        public GettextException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
