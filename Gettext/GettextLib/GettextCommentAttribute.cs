using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GettextCommentAttribute : Attribute
    {
        public string Comment { get; protected set; }

        public GettextCommentAttribute(string comment)
        {
            Comment = comment;
        }
    }
}
