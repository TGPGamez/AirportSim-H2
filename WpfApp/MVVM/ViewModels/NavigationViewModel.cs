using AirportLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.MVVM.Commands;

namespace WpfApp.MVVM.ViewModels
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        public Simulator Simulator { get; private set; }
        public ICommand LogsViewCommand { get; set; }
        public ICommand OverViewCommand { get; set; }

        private object selectedViewModel;
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }

        private LogsViewModel logsViewModel;
        private FlightScheduleViewModel flightScheduleViewModel;

        public NavigationViewModel(Simulator simulator)
        {
            Simulator = simulator;
            logsViewModel = new LogsViewModel(Simulator);
            flightScheduleViewModel = new FlightScheduleViewModel(Simulator.FlightSchedule);
            LogsViewCommand = new BaseCommand(OpenLogs);
            OverViewCommand = new BaseCommand(OpenOverView);
            SelectedViewModel = flightScheduleViewModel;
        }

        private void OpenLogs(object obj)
        {
            SelectedViewModel = logsViewModel;
        }

        private void OpenOverView(object obj)
        {
            SelectedViewModel = flightScheduleViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
