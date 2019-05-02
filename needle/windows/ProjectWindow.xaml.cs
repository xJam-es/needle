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
using needle.classes;
using needle.projectUC;

namespace needle.windows
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        public project _project;
        public MainWindow _mainWindow;
        internal static globalClass _gcl = new globalClass();

        // Tabs
        projectSummary pc;
        projectRAM pr;

        public ProjectWindow()
        {
            InitializeComponent();
        }

        public void initWindow()
        {
            Title = string.Format("[{0}] {1}",_project.projectID, _project.projectName);
            foreach (TabItem _tab in tabProject.Items)
            {
                string tabText = _tab.Header.ToString();
                // Create
                pc = new projectSummary(this);
                pr = new projectRAM();
                // Attach
                if (tabText == "Summary") _tab.Content = pc;
                if (tabText == "RAM Files") _tab.Content = pr;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindow._projectWindows.Remove(this);
        }

        private void mnuProjectSave_Click(object sender, RoutedEventArgs e)
        {
            _gcl.saveProject(_project);
        }
    }
}
