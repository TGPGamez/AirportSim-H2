using AirportLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.ViewModel
{
    class NavigationViewModel : INotifyPropertyChanged
    {

        public ICommand OverViewCommand { get; set; }
        public ICommand LogViewCommand { get; set; }

        //public ICommand DeptCommand { get; set; }

        private object selectedViewModel;

        public object SelectedViewModel

        {

            get { return selectedViewModel; }

            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }

        }



        public NavigationViewModel()
        {


            OverViewCommand = new BaseCommand(OpenOverView);

            LogViewCommand = new BaseCommand(OpenLogView);

        }

        private void OpenOverView(object obj)

        {

            SelectedViewModel = new OverviewViewModel();

        }

        private void OpenLogView(object obj)

        {

            SelectedViewModel = new ConsoleViewModel();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)

        {

            if (PropertyChanged != null)

            {

                PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }

        }
    }
}
