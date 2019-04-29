using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // APP Version
        private string _appversion = "";
        public string appversion
        {
            get { return _appversion; }
            set { _appversion = value; }
        }
    }
}
