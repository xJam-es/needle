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
using System.Diagnostics;
using System.Net;
using needle.windows;
using needle.classes;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Collections.ObjectModel;

namespace needle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        internal static globalClass _gcl = new globalClass();
        public LogWindow _log = new LogWindow();
        internal static OptionsWindow _options = new OptionsWindow();
        internal static NewProjectWindow _newProject = new NewProjectWindow();
        public ObservableCollection<project> _projectCollection = new ObservableCollection<project>();
        public List<ProjectWindow> _projectWindows = new List<ProjectWindow>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // load Settings
            _options._mainWindow = this;
            _options._log = _log;
            _options.loadOptions();

            // Setup New Project
            _newProject._mainWindow = this;
            _newProject._mainOptions = _options._mainOptions;
            _newProject.createNewProject();

            // Auto Update
            _log.addtolog("App Version: " + _gcl.appVersion);
            this.Title = _gcl.appName;
            var updateTask = new Task(checkforUpdates);
            updateTask.Start();

            // Enumerate Projects
            loadProjectsList();
        }

        public void checkforUpdates()
        {
            if (_options.autoUpdateChecked)
            {
                _log.addtolog("Checking for Updates");
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                          "Windows NT 5.2; .NET CLR 1.0.3705;)");
                        var json = client.DownloadString(_gcl.github_api);
                        dynamic jsRes = JsonConvert.DeserializeObject(json);
                        if (jsRes.tag_name.Value == _gcl.appVersion) _log.addtolog("Application is up to date");
                        else
                        {
                            _log.addtolog(_gcl.LOG_TYPE_HIGHLIGHT, "New version available: " + jsRes.tag_name.Value);
                            MessageBoxResult mbr = MessageBox.Show(string.Format("New Version Available: v{0}:\r\n{1}\r\nWould you like to visit the Download page?", jsRes.tag_name.Value, jsRes.tag_name.Value), "New Version Available", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if( mbr == MessageBoxResult.Yes ) System.Diagnostics.Process.Start(_gcl.github_latest);
                        }

                    }
                    catch (WebException e)
                    {
                        _log.addtolog(_gcl.LOG_TYPE_ERROR, e.Message);
                    }
                }
            }

        }

        public void loadProjectsList()
        {
            projectCollection.Clear();
            if (Directory.Exists(_options._mainOptions.folderProjects))
            {
                string[] fileList = Directory.GetFiles(_options._mainOptions.folderProjects,"*.npr");
                _log.addtolog(string.Format("{0} project file{1} found",fileList.Length,(fileList.Length==1) ? "" : "s"));
                foreach( string fileName in fileList)
                {
                    try
                    {
                        project _tmp = _gcl.loadProject(fileName);
                        projectCollection.Add(_tmp);
                    }
                    catch (Exception) { _log.addtolog(_gcl.LOG_TYPE_WARNING, string.Format("Invalid project file '{0}'", fileName)); }
                }
            }
            else _log.addtolog(_gcl.LOG_TYPE_ERROR,string.Format("Projects Directory Not Found '{0}'", _options._mainOptions.folderProjects));
        }

        #region Data Bindings
        public ObservableCollection<project> projectCollection
        {
            get { return _projectCollection; }
            set { _projectCollection = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Main Window Events
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); // close all windows
        }

        #endregion

        #region Menu Click Events

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Under Construction");
        }

        private void mnuMainExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuShowLog(object sender, RoutedEventArgs e)
        {
            _log.Show();
            if (_log.WindowState == WindowState.Minimized) _log.WindowState = WindowState.Normal;
            _log.Activate();
        }

        private void mnuOpenWiki(object sender, RoutedEventArgs e)
        {
            Process.Start(_gcl.github_wiki);
        }

        private void mnuOpenProject(object sender, RoutedEventArgs e)
        {
            Process.Start(_gcl.github_home);
        }

        private void mnuOptions(object sender, RoutedEventArgs e)
        {
            _options.ShowDialog();
        }

        private void mnuNewProject(object sender, RoutedEventArgs e)
        {
            _newProject.ShowDialog();
            loadProjectsList();
        }

        #endregion

        private void ListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            lstMainOpen.IsEnabled = lstMain.SelectedItems.Count > 0;
            lstMainRemove.IsEnabled = lstMain.SelectedItems.Count > 0;
        }

        private void lstMainRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult _mbr = MessageBox.Show(string.Format("Are you sure you want to remove {0} project{1}?", lstMain.SelectedItems.Count, lstMain.SelectedItems.Count == 1 ? "" : "s"), "Confirm Remove",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(_mbr == MessageBoxResult.Yes) foreach(project _selected in lstMain.SelectedItems)
            {
                    _log.addtolog(_gcl.LOG_TYPE_HIGHLIGHT, "Removing Project: " + _selected.projectID);
                    File.Delete(_selected.saveFullName);
            }
            loadProjectsList();
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            mnuMainOpen.IsEnabled = (lstMain.SelectedItems.Count > 0);
        }

        private void mnuMainOpen_Click(object sender, RoutedEventArgs e)
        {
            foreach( project _project in lstMain.SelectedItems)
            {
                foreach( ProjectWindow _win in _projectWindows)
                {
                    if (_win._project.projectID == _project.projectID)
                    {
                        _win.Show();
                        if (_win.WindowState == WindowState.Minimized) _win.WindowState = WindowState.Normal;
                        _win.Activate();
                        return;
                    }
                }
                _log.addtolog(_gcl.LOG_TYPE_HIGHLIGHT,string.Format("Creating Project Window [{0}] {1}",_project.projectID,_project.projectName));
                ProjectWindow _projectWin = new ProjectWindow();
                _projectWin._mainWindow = this;
                _projectWin._project = _project;
                _projectWin.Show();
                _projectWin.initWindow();
                _projectWindows.Add(_projectWin);
            }
        }

        private void lstMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            mnuMainOpen_Click(sender, new RoutedEventArgs());
        }
    }
}
