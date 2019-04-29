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
using System.IO;
using Microsoft.Win32;
using needle.classes;

namespace needle.windows
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        internal static globalClass _gcl = new globalClass();

        public LogWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void addtolog( string message)
        {
            addtolog(0, message);
        }

        public void addtolog(int logType, string message)
        {
            // Brush Colour
            object bru = new object();
            bru = Brushes.Black;
            string prefix = ".";
            if (logType == _gcl.LOG_TYPE_ERROR) { bru = Brushes.Red; prefix = "x";  }
            if (logType == _gcl.LOG_TYPE_WARNING) { bru = Brushes.DarkOrange; prefix = "!"; }
            if (logType == _gcl.LOG_TYPE_SUCCESS) { bru = Brushes.DarkGreen; prefix = ";"; }
            if (logType == _gcl.LOG_TYPE_HIGHLIGHT) { bru = Brushes.RoyalBlue; prefix = ":"; }
            // Create Text
            TextRange tr = new TextRange(logBox.Document.ContentEnd, logBox.Document.ContentEnd);
            tr.Text = string.Format("{0}[{1}] {2}\r", prefix, DateTime.Now.ToShortTimeString(), message);
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, bru);
        }

        public void clearLog()
        {
            logBox.Document.Blocks.Clear();
        }
        private void Window_Initialized(object sender, EventArgs e)
        {
            clearLog();
            addtolog("Application Started: " + DateTime.Today.ToShortDateString());
        }

        private void mnuClearLog(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to clear the log?", "Clear log confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.OK) clearLog();
        }

        private void mnuSaveLog(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Log file (*.log)|*.log|Any file (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                // File.WriteAllText(saveFileDialog.FileName, logBox.Document.Blocks.ToString());
                TextRange range;
                FileStream fStream;
                range = new TextRange(logBox.Document.ContentStart, logBox.Document.ContentEnd);
                fStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                range.Save(fStream, DataFormats.Text);
                fStream.Close();
            }
        }
    }
}
