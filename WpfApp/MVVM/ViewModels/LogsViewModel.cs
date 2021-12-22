using AirportLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp.MVVM.ViewModels
{
    public class LogsViewModel : ViewModelBase
    {
        public Simulator Simulator { get; private set; }

        private readonly ObservableCollection<string> generalInfo;
        public IEnumerable<string> GeneralLog => generalInfo;

        private readonly ObservableCollection<string> sorterInfo;
        public IEnumerable<string> SorterLog => sorterInfo;

        public LogsViewModel(Simulator simulator)
        {
            generalInfo = new ObservableCollection<string>();
            sorterInfo = new ObservableCollection<string>();

            Simulator = simulator;

            Simulator.ExceptionInfo += OnGeneralErrorInfo;
            Simulator.FlightSchedule.ScheduleInfo += OnGeneralInfo;
            Simulator.FlightSchedule.ScheduleException += OnGeneralWarningInfo;
            Simulator.Sorter.SortingExceptionInfo += OnSorterProcessError;
            Simulator.Sorter.SortingInfo += OnSorterProcessInfo;
        }

        private void OnGeneralErrorInfo(string msg)
        {
            InsertGeneralLog($"[ERROR] {msg}");
        }
        private void OnGeneralWarningInfo(string msg)
        {
            InsertGeneralLog($"[WARNING] {msg}");
        }
        private void OnGeneralInfo(string msg)
        {
            InsertGeneralLog($"{msg}");
        }
        private void OnSorterProcessError(string msg)
        {
            InsertSorterLog($"[ERROR] {msg}");
        }
        private void OnSorterProcessInfo(string msg)
        {
            InsertSorterLog($"{msg}");
        }


        private void InsertGeneralLog(string logMessage)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                generalInfo.Insert(0, logMessage);
                if (generalInfo.Count > 10)
                {
                    generalInfo.RemoveAt(generalInfo.Count - 1);
                }
            });
        }

        private void InsertSorterLog(string logMessage)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                sorterInfo.Insert(0, logMessage);
                if (sorterInfo.Count > 10)
                {
                    sorterInfo.RemoveAt(sorterInfo.Count - 1);
                }
            });
        }
    }
}
