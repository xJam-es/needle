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

namespace needle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static globalClass _gcl = new globalClass();
        public LogWindow _log = new LogWindow();
        internal static OptionsWindow _options = new OptionsWindow();

        public MainWindow()
        {
            InitializeComponent();

            // load Settings
            _options._mainWindow = this;
            _options._log = _log;
            _options.loadOptions();

            // Auto Update
            _log.addtolog("App Version: " + _gcl.appVersion);
            this.Title = _gcl.appName;
            var updateTask = new Task(checkforUpdates);
            updateTask.Start();
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
                        debugWrite(e.Message);
                    }
                }
            }

        }

        private void debugWrite(string str)
        {
           _log.addtolog(_gcl.LOG_TYPE_ERROR,str);
        }

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
            System.Diagnostics.Process.Start(_gcl.github_wiki);
        }

        private void mnuOpenProject(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(_gcl.github_home);
        }

        private void mnuOptions(object sender, RoutedEventArgs e)
        {
            _options.ShowDialog();
        }

        #endregion

        #region Main Window Events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); // close all windows
        }

        #endregion
    }
}
