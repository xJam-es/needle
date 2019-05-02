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
using needle.classes;
using needle.windows;

namespace needle.projectUC
{
    /// <summary>
    /// Interaction logic for projectSummary.xaml
    /// </summary>
    public partial class projectSummary : UserControl, INotifyPropertyChanged
    {
        public project _projectPrivate;
        public ProjectWindow _pwin;

        public projectSummary(ProjectWindow _pwinTmp)
        {
            InitializeComponent();
            DataContext = this;

            _pwin = _pwinTmp;
            _project = _pwin._project;
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
        public project _project
        {
            get { return _projectPrivate; }
            set { _projectPrivate = value; NotifyPropertyChanged(); }
        }
        #endregion

    }
}
