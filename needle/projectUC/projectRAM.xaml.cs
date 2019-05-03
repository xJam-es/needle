using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using needle.classes;
using needle.windows;
using System.IO;

namespace needle.projectUC
{
    /// <summary>
    /// Interaction logic for projectRAM.xaml
    /// </summary>
    public partial class projectRAM : UserControl, INotifyPropertyChanged
    {
        ProjectWindow _projectWindow;
        LogWindow _log;
        globalClass _gcl = new globalClass();
        ObservableCollection<ramFile> _ramFiles = new ObservableCollection<ramFile>();

        public projectRAM(ProjectWindow projectWin)
        {
            _projectWindow = projectWin;
            _log = projectWin._mainWindow._log;
            InitializeComponent();
            initRamFiles(projectWin._project);
            DataContext = this;
        }

        private void initRamFiles(project _project)
        {
            _ramFiles = new ObservableCollection<ramFile>(_project.ramFiles);
            foreach (ramFile _rf in ramFiles.ToList())
            {
                _rf.addressStart = _project.projectPlatform.MEMRANGE_BEGIN;
                if (!File.Exists(_rf.fileName))
                {
                    ramFiles.Remove(_rf);
                    _log.addtolog(_gcl.LOG_TYPE_WARNING, string.Format("RAM File Not Found '{0}'", _rf.fileName));

                }
            }
        }

        #region UC Events
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data Bindings
        public ObservableCollection<ramFile> ramFiles
        {
            get { return _ramFiles; }
            set { _ramFiles = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Button Events
        private void btnAddRAM_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ramFile _tmpFile = new ramFile(openFileDialog.FileName, _projectWindow._project.projectPlatform.MEMRANGE_BEGIN);
                ramFiles.Add(_tmpFile);
            }
        }

        #endregion
    }
}
