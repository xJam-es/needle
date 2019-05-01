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
using System.Windows.Shapes;
using needle.windows;
using needle.classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;
using System.IO;

namespace needle.windows
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window, INotifyPropertyChanged
    {
        public MainWindow _mainWindow;
        public LogWindow _log;
        public mainOptions _mainOptions;
        internal static globalClass _gcl = new globalClass();

        public OptionsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void loadOptions()
        {
            // Load From File
            _log.addtolog("Loading Settings");
            try { _mainOptions = _gcl.loadMainOptions(new mainOptions().fileSettings); }
            catch (Exception e) {
                // Default Settings
                _log.addtolog(_gcl.LOG_TYPE_WARNING, e.Message);
                _mainOptions = new mainOptions();              
            }

            // Projects
            try { if (!Directory.Exists(_mainOptions.folderProjects)) Directory.CreateDirectory(_mainOptions.folderProjects); }
            catch (Exception ex) { _log.addtolog(_gcl.LOG_TYPE_ERROR, ex.Message); }
        }

        public void save()
        {
            try { _gcl.saveMainOptions(_mainOptions); }
            catch (Exception e) { _log.addtolog(_gcl.LOG_TYPE_ERROR, e.Message); }
        }

        public void browseForFolder( TextBox tb)
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.SelectedPath = tb.Text;
            dlg.ShowNewFolderButton = true;

            if (dlg.ShowDialog() ?? false)
            {
                tb.Text = dlg.SelectedPath;
            }
        }

        #region Window Events
        // Don't Forget DataContext = this;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loadOptions();
            // Data Bindings
            autoUpdateChecked = autoUpdateChecked;
            projectsPath = projectsPath;
            // Goodbye
            this.Hide();
            e.Cancel = true;
        }
        #endregion

        #region Data Bindings
        public bool autoUpdateChecked
        {
            get { return _mainOptions.autoUpdate; } 
          set
            {
                _mainOptions.autoUpdate = value;
                NotifyPropertyChanged();
            }
        }

        public string projectsPath
        {
            get { return _mainOptions.folderProjects; }
            set
            {
                _mainOptions.folderProjects = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Button Events
        private void btnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {
            save();
            this.Close();
        }

        private void btnProjectPath(object sender, RoutedEventArgs e)
        {
            browseForFolder(txtProjects);
        }

        #endregion


    }
}
