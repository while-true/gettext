using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GettextExtractDataAnnotations;

namespace GettextExtractorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Missing directory");
                return;
            }

            var d = args[0];
            if (string.IsNullOrWhiteSpace(d)) throw new Exception("Missing directory specification");
            var info = new DirectoryInfo(d);
            
            var files = Directory.GetFiles(info.FullName, "*.dll", SearchOption.AllDirectories);

            var ext = new Extractor();

            foreach (var file in files)
            {
                
                try
                {
                    //var a = Assembly.ReflectionOnlyLoadFrom(file);
                    
                    //var dd = AppDomain.CreateDomain("parse");
                    //var a = dd.Load(File.ReadAllBytes(file));

                    var a = Assembly.LoadFile(file);

                    ext.Parse(a);

                    Console.WriteLine("Parsed {0}", file);

                } catch(Exception e)
                {
                    Console.WriteLine("Error parsing {0}.\n{1}\n", file, e.Message);
                }
            }

            var poFile = "messages-data-annotations.pot";
            var enc = new UTF8Encoding(false);
            File.WriteAllText(poFile, ext.ToPoString(), enc);
            Console.WriteLine("Wrote " + poFile);

        }
    }
}
