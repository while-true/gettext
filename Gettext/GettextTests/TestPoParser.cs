﻿using System;
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
            //var f = File.ReadAllText("test-po\\test_basic_2.po");
            var f = File.ReadAllText("test-po\\test_utf8.po");
            //Console.WriteLine(f);

            //ScannerDump(f);

            var scanner = new Gettext.PoScanner.Scanner();
            scanner.SetSource(f, 0);
            
            var parser = new Gettext.PoScanner.Parser(scanner);
            parser.Parse();

            var catalog = parser.Catalog;

            if (catalog == null) throw new Exception("no catalog!");

            var a = 1;

        }

        private void ScannerDump(string str)
        {
            var scanner = new Gettext.PoScanner.Scanner();
            scanner.SetSource(str, 0);

            int tok;
            do
            {
                tok = scanner.yylex();
                if (Enum.IsDefined(typeof(Gettext.PoScanner.Tokens), tok))
                {
                    var t = (Gettext.PoScanner.Tokens) tok;

                    Console.WriteLine("Token " + t);
                }
            } while (tok > (int) Gettext.PoScanner.Tokens.EOF);
        }
    }
}
