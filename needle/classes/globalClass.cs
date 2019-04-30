using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
// using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace needle.classes
{
    class globalClass
    {
        // Log Types
        public int LOG_TYPE_NORMAL { get; } = 0; // .
        public int LOG_TYPE_ERROR { get; } = 1; // x
        public int LOG_TYPE_WARNING { get; } = 2; // !
        public int LOG_TYPE_SUCCESS { get; } = 3; // ;
        public int LOG_TYPE_HIGHLIGHT { get; } = 4; // :

        // URLs
        public string github_api { get; } = "https://api.github.com/repos/xJam-es/needle/releases/latest";
        public string github_home { get; } = "https://github.com/xJam-es/needle";
        public string github_wiki { get; } = "https://github.com/xJam-es/needle/wiki";
        public string github_latest { get; } = "https://github.com/xJam-es/needle/releases/latest";

        public string appDirectory { get; } = Directory.GetCurrentDirectory();

        // Private Variables
        private string _appVersion = "";
        private string _appName = "needle";

        public globalClass()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            _appVersion = fvi.FileMajorPart + "." + fvi.FileMinorPart;
        }

        // Get / Set
        public string appVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; }
        }

        public string appName
        {
            get { return _appName + " v" + appVersion; }
        }


        // Serialise / Deserialise
        private static void classToJson<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        private static T jsonToClass<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        // Class Saving / Loading
        public void saveMainOptions ( mainOptions _options)
        {
            classToJson(_options.fileSettings, _options);
        }
        public mainOptions loadMainOptions( string filePath)
        {
           return jsonToClass<mainOptions>(filePath);
        }
    }

    public class mainOptions
    {
        // Support
        private globalClass _gcl = new globalClass();

        // Private Variables
        private string _fileSettings;
        private string _folderProjects;
        private bool _autoUpdate;

        // Get/Set
        public string folderProjects {
            get { return _folderProjects; }
            set {
                if( Directory.Exists(value) ) _folderProjects = value;
            }
        }
        public string fileSettings { get { return _fileSettings; } }
        public bool autoUpdate { get { return _autoUpdate; } set { _autoUpdate = value; } }

        // Constructor
        public mainOptions()
        {
            _fileSettings = _gcl.appDirectory + "\\settings.json";
            _folderProjects = _gcl.appDirectory + "\\projects";
            _autoUpdate = true;
        }

    }


}
