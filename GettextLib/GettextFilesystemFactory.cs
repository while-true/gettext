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
    public class GettextFilesystemFactory : GettextFactoryBase, IDisposable
    {
        private readonly string poDirectory;
        private readonly LocaleFileOrganizationEnum fileOrganization;
        private Dictionary<string, LanguageTranslation> catalogs;
        private readonly FileSystemWatcher fileSystemWatcher;

        public enum LocaleFileOrganizationEnum
        {
            /// <summary>
            /// Translations are stored in files named after cultures, all in the poDirectory.
            /// 
            /// Samples: en-US.po, sl-SI.po, ...
            /// </summary>
            FilePerLocale,
            /// <summary>
            /// Translations are stored in a message.po in a directory named after the locale.
            /// 
            /// Samples: en-US/messages.po, sl-SI/messages.po, ...
            /// </summary>
            DirectoryPerLocale
        }

        /// <summary>
        /// Lock when retrieving or modifying the translation collection.
        /// </summary>
        private object lockObject = new object();

        public GettextFilesystemFactory([NotNull] string poDirectory, LocaleFileOrganizationEnum fileOrganization) : this()
        {
            if (poDirectory == null) throw new ArgumentNullException("poDirectory");
            this.poDirectory = poDirectory;
            this.fileOrganization = fileOrganization;
            if (string.IsNullOrWhiteSpace(poDirectory)) throw new GettextException("Missing directory parameter");

            var d = new DirectoryInfo(poDirectory);
            if (!d.Exists) throw new GettextException("Directory doesn't exist!");

            // initial load
            {
                var c = LoadAllLanguages(d, fileOrganization);

                lock (lockObject)
                {
                    LoadIntoCatalogs(c, catalogs);
                }
            }

            // filesystem watcher
            {
                var includeSubdirectories = false;
                var filePattern = "*.po";

                if (fileOrganization == LocaleFileOrganizationEnum.DirectoryPerLocale)
                {
                    filePattern = "messages.po";
                    includeSubdirectories = true;
                }

                // init
                fileSystemWatcher = new FileSystemWatcher(d.FullName, filePattern);
                fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                fileSystemWatcher.IncludeSubdirectories = includeSubdirectories;

                fileSystemWatcher.Changed += FileSystemWatcherEventHandler;
                fileSystemWatcher.Created += FileSystemWatcherEventHandler;
                fileSystemWatcher.Deleted += FileSystemWatcherEventHandler;
                fileSystemWatcher.Renamed += FileSystemWatcherEventHandler;

                fileSystemWatcher.EnableRaisingEvents = true;
            }
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
            var languages = LoadAllLanguages(d, fileOrganization);

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

        private static LanguageTranslation LoadCatalog([NotNull] FileInfo f, LocaleFileOrganizationEnum fileOrganization)
        {
            if (f == null) throw new ArgumentNullException("f");
            if (!f.Exists) throw new GettextException("File " + f.FullName + " doesn't exist anymore!");
            
            string langId = null;
            if (fileOrganization == LocaleFileOrganizationEnum.FilePerLocale)
            {
                langId = f.Name.Replace(".po", "").Trim();
            } else
            {
                if (f.Directory != null) langId = f.Directory.Parent.Name.Trim();
            }

            if (langId == null) throw new GettextException("Language id for file " + f.FullName + " can't be determined.");


            using (var fs = f.OpenRead())
            {
                var catalog = GettextCatalog.ParseFromStream(fs);

                var lt = new LanguageTranslation {LangId = langId, Culture = CultureInfo.GetCultureInfo(langId), Gettext = new Gettext(catalog)};

                return lt;
            }
        }

        private static List<LanguageTranslation> LoadAllLanguages([NotNull] DirectoryInfo d, LocaleFileOrganizationEnum fileOrganization)
        {
            if (d == null) throw new ArgumentNullException("d");
            if (!d.Exists) throw new GettextException("Directory " + d.FullName + " doesn't exist!");

            var f = d.GetFiles(fileOrganization == LocaleFileOrganizationEnum.FilePerLocale ? "*.po" : "messages.po", 
                               fileOrganization == LocaleFileOrganizationEnum.FilePerLocale ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);

            return f.Select(info => LoadCatalog(info, fileOrganization)).ToList();
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

        public override GettextTranslationContext GetContext(string langId)
        {
            if (string.IsNullOrWhiteSpace(langId) || langId == GettextConsts.GettextNullLanguage)
            {
                return GetNullContext();
            }

            if (langId == GettextConsts.GettextPseudoLanguage)
            {
                return GetPseudoContext();
            }

            lock (lockObject)
            {
                LanguageTranslation l;
                if (catalogs.TryGetValue(langId, out l))
                {
                    return new GettextTranslationContext(l);
                }

                return GetNullContext();
            }
        }
    }
}
