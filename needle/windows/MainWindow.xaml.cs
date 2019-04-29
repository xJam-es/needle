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

namespace needle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static globalClass _gcl = new globalClass();
        internal static LogWindow _log = new LogWindow();

        public MainWindow()
        {
            InitializeComponent();
            getAppVersion();
            checkforUpdates().Wait();
        }

        private void getAppVersion ()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            _gcl.appversion = fvi.FileMajorPart + "." + fvi.FileMinorPart;
            _log.addtolog("App Version: " + _gcl.appversion);
        }

        public static async Task checkforUpdates()
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
                    if (jsRes.tag_name.Value == _gcl.appversion) _log.addtolog("Application is up to date");
                    else
                    {
                        _log.addtolog(_gcl.LOG_TYPE_HIGHLIGHT,"New version available: " + jsRes.tag_name.Value);
                    }
                    
                } catch(WebException e)
                {
                    debugWrite(e.Message);
                }
            }

        }

        private static void debugWrite(string str)
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

        #endregion

        #region Main Window Events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); // close all windows
        }

        #endregion


    }
}
