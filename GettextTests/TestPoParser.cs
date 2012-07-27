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
        public void TestParser()
        {
            var d = new DirectoryInfo("test-po");
            var files = d.GetFiles("*.po");

            foreach (var fileInfo in files)
            {
                Console.WriteLine(fileInfo.FullName);
                using (var f = fileInfo.OpenRead())
                {

                    try
                    {
                        var catalog = GettextCatalog.ParseFromStream(f);

                        if (catalog == null) throw new Exception("no catalog!");

                        Console.WriteLine("translations: " + catalog.Translations.Count);

                        if (fileInfo.Name.Contains("comment-last.po"))
                        {
                            var t = catalog.Translations.SingleOrDefault(x => x.MessageId.String == "foo");
                            Assert.That(t, Is.Not.Null);
                            Assert.That(t.Fuzzy, Is.True);
                        }
                    } catch (Exception)
                    {
                        throw;
                        //Console.WriteLine("CRASH");
                    }
                }
            }
        }

        [Test]
        public void TestBasic2()
        {
            //using (var f = new FileInfo("test-po/test_basic_2.po").OpenRead())
            using (var f = new FileInfo("test-po/comment-last.po").OpenRead())
            {
                var catalog = GettextCatalog.ParseFromStream(f);
            }
        }

        [Test]
        public void TestCommentOnly()
        {
            using (var f = new FileInfo("test-po/comment-last.po").OpenRead())
            //using (var f = new FileInfo("test-po/comment-only.po").OpenRead())
            //using (var f = new FileInfo("test-po/test_basic_2.po").OpenRead())
            {
                ScannerDump(f);
            }
        }

        private void ScannerDump(Stream s)
        {
            var scanner = new GettextLib.Parser.Scanner();
            scanner.SetSource(s);

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
        public void TestPoParserIso8859_15()
        {
            var f = new FileInfo("test-po/test_iso-8859-15.po");

            using (var fs = f.OpenRead())
            {
                var catalog = GettextCatalog.ParseFromStream(fs);

                Console.Write(catalog.Translations.Count);
            }
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
        public void TestPluralTranslations()
        {
            using (var fileStream = File.OpenRead("po\\test-plural-slo.po"))
            {
                var catalog = GettextCatalog.ParseFromStream(fileStream);
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
}
