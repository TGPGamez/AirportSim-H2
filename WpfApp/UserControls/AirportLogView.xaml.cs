using AirportLib;
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

namespace WpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for AirportLogView.xaml
    /// </summary>
    public partial class AirportLogView : UserControl
    {
        public Simulator Simulator { get; private set; }
        public AirportLogView()
        {
            InitializeComponent();
        }

        public void Initalize(Simulator simulator)
        {
            Simulator = simulator;
            
            Simulator.Sorter.SortingInfo += SorterInfo;
            Simulator.Sorter.SortingExceptionInfo += SorterExceptionInfo;

            Simulator.FlightSchedule.FlightInfo += GeneralInfo;
            Simulator.FlightSchedule.FlightExceptionInfo += SorterExceptionInfo;

            Simulator.ExceptionInfo += GeneralExceptionInfo;
        }

        private void GeneralInfo(string message) => AP_Con_GeneralInfo.WriteLogLine($"[INFO] {message}");
        private void GeneralExceptionInfo(string message) => AP_Con_GeneralInfo.WriteLogLine($"[ERROR] {message}");

        private void SorterInfo(string message) => AP_Con_SorterInfo.WriteLogLine($"[INFO] {message}");
        private void SorterExceptionInfo(string message) => AP_Con_SorterInfo.WriteLogLine($"[ERROR] {message}");
    }
}
