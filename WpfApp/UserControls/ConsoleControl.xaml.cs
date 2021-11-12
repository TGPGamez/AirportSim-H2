using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WpfApp.UserControls
{
    public partial class ConsoleControl : UserControl
    {
        public ObservableCollection<string> Lines { get; set; }

        public ConsoleControl()
        {
            InitializeComponent();

            Lines = new ObservableCollection<string>();
            DataContext = this;
        }

        public void WriteLogLine(string line)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new Action(() =>
            {
                Lines.Insert(0, line);
                if (Lines.Count > 100)
                {
                    Lines.RemoveAt(Lines.Count - 1);
                }
            }));
        }
    }
}
