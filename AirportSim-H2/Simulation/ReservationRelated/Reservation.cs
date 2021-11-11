using AirportSim_H2.Simulation.FlightRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.ReservationRelated
{
    public class Reservation
    {
        public Passenger Passenger { get; private set; }
        public Flight Flight { get; private set; }
        public bool IsCheckedIn { get; set; }

        public Reservation(Passenger passenger, Flight flight)
        {
            Passenger = passenger;
            Flight = flight;
            IsCheckedIn = false;
        }
    }
}
