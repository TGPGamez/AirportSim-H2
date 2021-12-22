using AirportLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.MVVM.ViewModels
{
    public class FlightScheduleViewModel : ViewModelBase
    {
        //Need Updates when status is changed on a flight

        public FlightSchedule FlightSchedule { get; set; }
        private readonly ObservableCollection<Flight> flights;
        public IEnumerable<Flight> Flights => flights; 

        public FlightScheduleViewModel(FlightSchedule flightSchedule)
        {
            FlightSchedule = flightSchedule;
            flights = new ObservableCollection<Flight>();
            FlightSchedule.AddedFlight += OnFlightAdd;
            FlightSchedule.RemovedFlight += OnFlightRemove;
            
            //Simulator.FlightSchedule
        }

        private void OnFlightRemove(Flight flight)
        {
            flights.Remove(flight);
        }

        private void OnFlightAdd(Flight flight)
        {
            flights.Add(flight);
        }
    }
}
