using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GettextLib;
using GettextLib.Catalog;
using GettextLib.ExpressionEvaluator;
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

        [Test]
        public void TestExpressionExecution()
        {
            var expressions = new[]
                               {
                                   
"plural=n == 1 ? 0 : 1;",
"plural=0;",
"plural=n != 1;",
"plural=n>1;",
"plural=n%10==1 && n%100!=11 ? 0 : n != 0 ? 1 : 2;",
"plural=n==1 ? 0 : n==2 ? 1 : 2;",
"plural=n==1 ? 0 : (n==0 || (n%100 > 0 && n%100 < 20)) ? 1 : 2;",
"plural=n%10==1 && n%100!=11 ? 0 : n%10>=2 && (n%100<10 || n%100>=20) ? 1 : 2;",
"plural=n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2;",
"plural=(n==1) ? 0 : (n>=2 && n<=4) ? 1 : 2;",
"plural=n==1 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2;",
"plural=n%100==1 ? 0 : n%100==2 ? 1 : n%100==3 || n%100==4 ? 2 : 3;"
                        

                                  };


            var testsFunc = new Func<int, int>[]
                            {
                                n => n == 1 ? 0 : 1,
                                n => 0,
                                n => (n != 1) ? 1 : 0,
                                n => (n > 1) ? 1 : 0,
                                n => n%10 == 1 && n%100 != 11 ? 0 : n != 0 ? 1 : 2,
                                n => n == 1 ? 0 : n == 2 ? 1 : 2,
                                n => n == 1 ? 0 : (n == 0 || (n%100 > 0 && n%100 < 20)) ? 1 : 2,
                                n => n%10 == 1 && n%100 != 11 ? 0 : n%10 >= 2 && (n%100 < 10 || n%100 >= 20) ? 1 : 2,
                                n =>
                                n%10 == 1 && n%100 != 11
                                    ? 0
                                    : n%10 >= 2 && n%10 <= 4 && (n%100 < 10 || n%100 >= 20) ? 1 : 2,
                                n => (n == 1) ? 0 : (n >= 2 && n <= 4) ? 1 : 2,
                                n => n == 1 ? 0 : n%10 >= 2 && n%10 <= 4 && (n%100 < 10 || n%100 >= 20) ? 1 : 2,
                                n => n%100 == 1 ? 0 : n%100 == 2 ? 1 : n%100 == 3 || n%100 == 4 ? 2 : 3
                            };


            for (var i = 0; i < expressions.Length; i++)
            {
                var sc = new Scanner();
                sc.SetSource(expressions[i], 0);

                var p = new Parser(sc);

                p.Parse();

                var scr = p.Script;

                for (var j = 0; j < 1000; j++)
                {
                    var state = new ExpressionState();
                    state.SetVar("n", j);
                    scr.Execute(state);
                    var ours = state.GetVar("plural");

                    var b = testsFunc[i](j);

                    Assert.That(ours, Is.EqualTo(b));
                }
            }

        }

        [Test]
        public void TestExpressionParser()
        {
            var expressions = new[]
                               {
                                   /*
"nplurals=2;",
"nplurals=2; plural=n == 1 ? 0 : 1;",
"nplurals=1; plural=0;",
"nplurals=2; plural=n != 1;",
"nplurals=2; plural=n>1;",
"nplurals=3; plural=n%10==1 && n%100!=11 ? 0 : n != 0 ? 1 : 2;",
"nplurals=3; plural=n==1 ? 0 : n==2 ? 1 : 2;",
"nplurals=3; plural=n==1 ? 0 : (n==0 || (n%100 > 0 && n%100 < 20)) ? 1 : 2;",
"nplurals=3; plural=n%10==1 && n%100!=11 ? 0 : n%10>=2 && (n%100<10 || n%100>=20) ? 1 : 2;",
"nplurals=3; plural=n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2;",
"nplurals=3; plural=(n==1) ? 0 : (n>=2 && n<=4) ? 1 : 2;",
"nplurals=3; plural=n==1 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2;",
"nplurals=4; plural=n%100==1 ? 0 : n%100==2 ? 1 : n%100==3 || n%100==4 ? 2 : 3;"
                                    */


                                   "plural= n%100==1 ? 0 : n%100==2 ? 1 : 3;"

                                  };




            foreach (var expression in expressions)
            {
                Console.WriteLine(expression);
                Console.WriteLine("====");

                //ScannerEvalDump(expression);

                {
                    var sc = new Scanner();
                    sc.SetSource(expression, 0);

                    var p = new Parser(sc);

                    p.Parse();

                    var scr = p.Script;
                    Console.WriteLine(scr.ToPrint());

                    var s = new ExpressionState();
                    s.SetVar("n", 2);
                    scr.Execute(s);
                    Console.WriteLine(s.PrintState());
                }


                Console.WriteLine("====");
                Console.WriteLine();
                Console.WriteLine();
            }

            
        }
        
        private void ScannerEvalDump(string str)
        {
            var scanner = new GettextLib.ExpressionEvaluator.Scanner();
            scanner.SetSource(str, 0);

            int tok;
            do
            {
                tok = scanner.yylex();
                if (Enum.IsDefined(typeof(GettextLib.ExpressionEvaluator.Tokens), tok))
                {
                    var t = (GettextLib.ExpressionEvaluator.Tokens)tok;

                    Console.WriteLine("Token " + t);
                }
            } while (tok > (int)GettextLib.ExpressionEvaluator.Tokens.EOF);
        }


        [Test]
        public void TestPluralTranslations()
        {
            var catalog = GettextCatalog.ParseFromPoString(File.ReadAllText("po\\test-plural-slo.po"));
            var gt = new Gettext(catalog);

            {
                var msgid = "{0} file";
                var msgidPlural = "{0} files";

                var t = new[]
                            {
                                0, 1, 2, 3, 4, 5,
                                100, 101, 102, 103, 104, 105
                            };

                foreach (var i in t)
                {
                    var s = string.Format(gt.NGettext(msgid, msgidPlural, i), i);
                    Console.WriteLine(s);
                }
            }
        }

    }
}
