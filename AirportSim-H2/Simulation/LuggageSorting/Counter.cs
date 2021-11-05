using AirportSim_H2.Simulation.FlightRelated;
using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.LuggageSorting
{
    public class Counter
    {
        public int ID { get; private set; }
        public Luggage Luggage { get; private set; }
        public bool IsOpen { get; private set; }
        public Flight Flight { get; private set; }

        public Counter(int id)
        {
            ID = id;
            Luggage = null;
            IsOpen = false;
            Flight = Flight.Empty;         
        }

        internal void Open()
        {
            IsOpen = true;
        }

        internal void Close()
        {
            IsOpen = false;
            Flight = Flight.Empty;
        }

        internal void CheckIn(Luggage luggage)
        {
            if (Luggage == null)
            {
                Luggage = luggage;
            }
        }

        internal bool IsLuggageReady()
        {
            return (IsOpen) && (Luggage != null);
        }

        internal void SetCurrentFlight(Flight flight)
        {
            Flight = flight;
        }

        internal Luggage GetCurrentLuggage()
        {
            Luggage luggage = Luggage;
            Luggage = null;
            return luggage;
        }
    }
}
