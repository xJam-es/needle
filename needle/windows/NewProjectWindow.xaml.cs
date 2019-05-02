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
using System.ComponentModel; // INotifyPropertyChanged
using System.Runtime.CompilerServices; //INotifyPropertyChanged
using needle.classes;

namespace needle.windows
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window, INotifyPropertyChanged
    {
        internal static globalClass _gcl = new globalClass();
        public project _newProject;
        public platformCollection _plats = new platformCollection();
        public mainOptions _mainOptions;
        public MainWindow _mainWindow;

        public NewProjectWindow()
        {
            InitializeComponent();
            createNewProject();
            DataContext = this;
        }

        public void createNewProject()
        {
            _newProject = new project();
            if (_mainOptions != null) _newProject.savePath = _mainOptions.folderProjects;
            pokeBindings();
        }

        #region Data Bindings

        public project projectItem {
            get { return _newProject; }
            set {
                _newProject = value;
                NotifyPropertyChanged();
            }
        }
        public List<platformSingle> platforms { get { return _plats.list; } }

        private void pokeBindings()
        {
            projectItem = projectItem;
        }

        #endregion

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
            e.Cancel = true;
            createNewProject();
            cmbPlatform.SelectedIndex = 0;
            this.Hide();
        }
        #endregion

        #region Button Events
        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreateClick(object sender, RoutedEventArgs e)
        {
            // save
            _mainWindow._log.addtolog(_gcl.LOG_TYPE_SUCCESS,"Creating Project: " + _newProject.projectID);
            _gcl.saveProject(_newProject);
            this.Close();
        }

        private void btnNewIDClick(object sender, RoutedEventArgs e)
        {
            _newProject.generateNewID();
            pokeBindings();
        }
        #endregion

        private void cmbPlatform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtMemRegion.IsEnabled = cmbPlatform.SelectedIndex == 0;
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox _sender = sender as TextBox;
            char[] arr = _sender.Text.Where(c => (char.IsLetterOrDigit(c) ||
                             char.IsWhiteSpace(c) ||
                             c == '-')).ToArray();
            _sender.Text = new string(arr);
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(((e.Key >= Key.A) && (e.Key <= Key.F)) || ((e.Key >= Key.D0) && (e.Key <= Key.D9))))
                e.Handled = true;
        }
    }
}
