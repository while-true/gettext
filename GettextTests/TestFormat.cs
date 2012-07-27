using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettextLib;
using NUnit.Framework;

namespace GettextTests
{
    [TestFixture]
    public class TestFormat
    {
        [Test]
        public void T()
        {
            var g = new GettextTranslatedString("Hello {world}!").FormatWithNamed(new {world = "WORLD"});
            StringAssert.AreEqualIgnoringCase("Hello WORLD!", g);
        }
    }
}
