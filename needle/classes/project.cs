using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace needle.classes
{
    public class project
    {
        // Support
        private globalClass _gcl = new globalClass();
        private platformCollection _pla = new platformCollection();
        static Random random = new Random();

        private string _projectID;
        private string _projectName;
        private platformSingle _projectPlatform;
        private string _projectGame;
        private string _appVersion;
        private string _filename;
        private List<ramFile> _ramFiles = new List<ramFile>();
        
        private string _savePath;

        // Constructor
        public project()
        {
            generateNewID();
            _projectName = "New Project";
            _projectPlatform = new platformSingle();
            _projectGame = "New Game";
            _appVersion = _gcl.appVersion;
        }

        // Public Data
        public string projectID
        {
            get { return _projectID; }
            set { _projectID = value; }
        }
        public string projectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        public string projectGame
        {
            get { return _projectGame; }
            set { _projectGame = value; }
        }
        public string platformMemBegin
        {
            get { return _projectPlatform.memRegionHex; }
            set { if (_projectPlatform.ID == 0) _projectPlatform.memRegionHex = value; }
        }
        public int platformSelected
        {
            get { return _projectPlatform.ID; }
            set { _projectPlatform = _pla.findById(value); }
        }
        public List<ramFile> ramFiles
        {
            get { return _ramFiles; }
            set { _ramFiles = value; }
        }

        // Non Serialised Public Data
        [JsonIgnore] public platformSingle projectPlatform
        {
            get { return _projectPlatform; }
            set { _projectPlatform = value; }
        }
        [JsonIgnore] public string appVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; }
        }
        [JsonIgnore] public string savePath
        {
            get { return _savePath; }
            set { _savePath = value; }
        }
        [JsonIgnore] public string saveFullName
        {
            get
            {
                if (_filename == null) return string.Format("{0}\\{1}.npr", savePath, projectID);
                else return _filename;
            }
            set
            {
                _filename = value;
            }
        }
       
        // Public Functions
        public void generateNewID()
        {
            _projectID = GetRandomHexNumber(8);
        }

        // Private Functions
        private static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }
    }
}
