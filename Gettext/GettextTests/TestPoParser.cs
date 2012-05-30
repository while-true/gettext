using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GettextLib;
using NUnit.Framework;

namespace GettextTests
{
    [TestFixture]
    public class TestPoParser
    {
        [Test]
        public void Test1()
        {
            //var f = File.ReadAllText("test-po\\test_basic_2.po");
            var f = File.ReadAllText("test-po\\test_utf8.po");
            //Console.WriteLine(f);

            //ScannerDump(f);

            var scanner = new GettextLib.Parser.Scanner();
            scanner.SetSource(f, 0);

            var parser = new GettextLib.Parser.Parser(scanner);
            parser.Parse();

            var catalog = parser.Catalog;

            if (catalog == null) throw new Exception("no catalog!");

            var a = 1;

        }

        private void ScannerDump(string str)
        {
            var scanner = new GettextLib.Parser.Scanner();
            scanner.SetSource(str, 0);

            int tok;
            do
            {
                tok = scanner.yylex();
                if (Enum.IsDefined(typeof(GettextLib.Parser.Tokens), tok))
                {
                    var t = (GettextLib.Parser.Tokens)tok;

                    Console.WriteLine("Token " + t);
                }
            } while (tok > (int)GettextLib.Parser.Tokens.EOF);
        }
    }
}
