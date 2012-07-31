using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GettextLib.Catalog;
using JetBrains.Annotations;

namespace GettextLib
{
    /// <summary>
    /// Automatically loads files from a dedicated po directory in your project.
    /// 
    /// Name of the file should specify the culture, like en-US.po for American English.
    /// 
    /// Monitors when files are changed, added or removed. Thread safe.
    /// </summary>
    public class GettextFilesystemFactory : IDisposable
    {
        private readonly string poDirectory;
        private Dictionary<string, LanguageTranslation> catalogs;
        private FileSystemWatcher fileSystemWatcher;

        /// <summary>
        /// Lock when retrieving or modifying the translation collection.
        /// </summary>
        private object lockObject = new object();

        public GettextFilesystemFactory([NotNull] string poDirectory) : this()
        {
            if (poDirectory == null) throw new ArgumentNullException("poDirectory");
            this.poDirectory = poDirectory;
            if (string.IsNullOrWhiteSpace(poDirectory)) throw new GettextException("Missing directory parameter");

            var d = new DirectoryInfo(poDirectory);
            if (!d.Exists) throw new GettextException("Directory doesn't exist!");

            // initial load
            {
                var c = LoadAllLanguages(d);

                lock (lockObject)
                {
                    LoadIntoCatalogs(c, catalogs);
                }
            }

            // set up watcher
            fileSystemWatcher = new FileSystemWatcher(d.FullName, "*.po");
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fileSystemWatcher.IncludeSubdirectories = false;
            
            fileSystemWatcher.Changed += FileSystemWatcherEventHandler;
            fileSystemWatcher.Created += FileSystemWatcherEventHandler;
            fileSystemWatcher.Deleted += FileSystemWatcherEventHandler;
            fileSystemWatcher.Renamed += FileSystemWatcherEventHandler;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void LoadIntoCatalogs([NotNull] IEnumerable<LanguageTranslation> l, [NotNull] Dictionary<string, LanguageTranslation> d)
        {
            if (l == null) throw new ArgumentNullException("l");
            if (d == null) throw new ArgumentNullException("d");

            foreach (var languageTranslation in l)
            {
                if (!d.ContainsKey(languageTranslation.LangId)) d.Add(languageTranslation.LangId, languageTranslation);
            }
        }

        private void FileSystemWatcherEventHandler(object source, FileSystemEventArgs e)
        {
            // also RenamedEventArgs
            var d = new DirectoryInfo(poDirectory);
            var languages = LoadAllLanguages(d);

            lock (lockObject)
            {
                catalogs = new Dictionary<string, LanguageTranslation>();

                LoadIntoCatalogs(languages, catalogs);
            }

        }

        private GettextFilesystemFactory()
        {
            catalogs = new Dictionary<string, LanguageTranslation>();
        }

        private static LanguageTranslation LoadCatalog([NotNull] FileInfo f)
        {
            if (f == null) throw new ArgumentNullException("f");
            if (!f.Exists) throw new GettextException("File " + f.FullName + " doesn't exist anymore!");
            
            var langId = f.Name.Replace(".po", "").Trim();

            using (var fs = f.OpenRead())
            {
                var catalog = GettextCatalog.ParseFromStream(fs);

                var lt = new LanguageTranslation {LangId = langId, Culture = CultureInfo.GetCultureInfo(langId), Gettext = new Gettext(catalog)};

                return lt;
            }
        }

        private static List<LanguageTranslation> LoadAllLanguages([NotNull] DirectoryInfo d)
        {
            if (d == null) throw new ArgumentNullException("d");

            var f = d.GetFiles("*.po", SearchOption.TopDirectoryOnly);

            return f.Select(LoadCatalog).ToList();
        }

        public void Dispose()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.Changed -= FileSystemWatcherEventHandler;
                fileSystemWatcher.Created -= FileSystemWatcherEventHandler;
                fileSystemWatcher.Deleted -= FileSystemWatcherEventHandler;
                fileSystemWatcher.Renamed -= FileSystemWatcherEventHandler;

                fileSystemWatcher.Dispose();
            }
        }
    }
}
