using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace needle.classes
{

    public class ramFile
    {
        // private globalClass _gcl = new globalClass();

        private string _fileName = "";
        private long _fileSize = -1;
        private long _addressStart = 0;

        public ramFile()
        {
            // blank constructor
        }

        public ramFile(string filePath)
        {
            fileName = filePath;
        }

        public ramFile(string filePath, long addStart )
        {
            fileName = filePath;
            addressStart = addStart;
        }

        public string fileName {
            get { return _fileName; }
            set {
                _fileName = value;
                if (File.Exists(value))
                    _fileSize = new FileInfo(value).Length;
                else _fileSize = -1;
            }
        }
        [JsonIgnore] public string fileNameShort
        {
            get { return Path.GetFileName(_fileName); }
        }
        [JsonIgnore] public long addressStart
        {
            get { return _addressStart; }
            set { _addressStart = value; }
        }
        [JsonIgnore] public long fileSize {
            get { return _fileSize; }
        }
        [JsonIgnore] public string fileSizeString
        {
            get {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = fileSize;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                return String.Format("{0:0.##} {1}", len, sizes[order]);

            }
        }
        [JsonIgnore] public long addressEnd {
            get { return _addressStart + _fileSize; }
        }
        [JsonIgnore] public string addressStartHex {
            get { return addressStart.ToString("X8"); }
        }
        [JsonIgnore] public string addressEndHex {
            get { return addressEnd.ToString("X8"); }
        }

    }
}
