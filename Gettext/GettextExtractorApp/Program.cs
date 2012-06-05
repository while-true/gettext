using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GettextExtractorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing directory/files");
                return;
            }


            var files = new List<FileInfo>();
            foreach (var s in args)
            {
                if (Directory.Exists(s))
                {
                    var d = Directory.GetFiles(s, "*.dll", SearchOption.AllDirectories);
                    files.AddRange(d.Select(x => new FileInfo(x)));

                } else if (File.Exists(s))
                {
                    files.Add(new FileInfo(s));
                } else
                {
                    throw new Exception("Not found: " + s);
                }
            }

            var ext = new Extractor();

            foreach (var file in files)
            {
                try
                {
                    var a = Assembly.LoadFile(file.FullName);

                    ext.Parse(a);

                } catch(Exception e)
                {
                    Console.WriteLine("Error parsing {0}.\n{1}\n", file, e.Message);
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(e.GetType());
                }
            }

            var poFile = "messages-data-annotations.pot";
            var enc = new UTF8Encoding(false);
            File.WriteAllText(poFile, ext.ToPoString(), enc);
            Console.WriteLine("Wrote " + poFile);
        }
    }
}
