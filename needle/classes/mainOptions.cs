using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace needle.classes
{

    public class mainOptions
    {
        // Support
        private globalClass _gcl = new globalClass();

        // Private Variables
        private string _fileSettings;
        private string _folderProjects;
        private bool _autoUpdate;

        // Get/Set
        public string folderProjects
        {
            get { return _folderProjects; }
            set
            {
                if (Directory.Exists(value)) _folderProjects = value;
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
