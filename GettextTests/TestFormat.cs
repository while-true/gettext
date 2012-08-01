using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void TestNamedFormatString()
        {
            {
                var t = new GettextTranslatedString("Hello {world}!").FormatWith(new {world = "WORLD"});
                Assert.That(t, Is.EqualTo("Hello WORLD!"));
            }

            {
                var t = new GettextTranslatedString("Hello {a.b}!").FormatWith(new {a = new {b = "WORLD"}});
                Assert.That(t, Is.EqualTo("Hello WORLD!"));
            }

            {
                var t = new GettextTranslatedString("Float: {floatValue:0.00}", CultureInfo.InvariantCulture).FormatWith(new { floatValue = 1.2345 });
                Assert.That(t, Is.EqualTo("Float: 1.23"));
            }
        }
    }
}
