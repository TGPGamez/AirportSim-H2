using AirportSim_H2.Simulation.FlightRelated;
using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.BaggageSorting
{
    public class Counter
    {
        public int ID { get; private set; }
        public Luggage Luggage { get; private set; }
        public Flight Flight { get; private set; }
        public bool IsOpen { get; private set; }

        public Counter(int id)
        {
            ID = id;
            Luggage = null;
            Flight = Flight.Empty;
            IsOpen = false;
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


        internal void CheckInLuggage(Luggage luggage)
        {
            if (Luggage == null)
            {
                Luggage = luggage;
            } else
            {
                //Log event -> already has luggage
            }
        }

        internal void AssignFlight(Flight flight)
        {
            Flight = flight;
        }

        internal Luggage GetLuggageFromCounter()
        {
            Luggage luggage = Luggage;
            Luggage = null;
            return luggage;
        }

        public bool IsLuggageReady()
        {
            return (IsOpen) && (Luggage != null);
        }
    }
}
