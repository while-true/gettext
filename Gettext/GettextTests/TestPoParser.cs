using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GettextTests
{
    [TestFixture]
    public class TestPoParser
    {
        [Test]
        public void Test1()
        {
            var f = File.ReadAllText("test-po\\test_indented.po");
            Console.WriteLine(f);

            var p = new Gettext.PoScanner.Scanner();
            p.SetSource(f, 0);

            p.yylex();
        }
    }
}
