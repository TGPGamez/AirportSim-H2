using System.Collections.Generic;

namespace AirportLib
{
    public class Gate
    {
        public int ID { get; private set; }
        public bool IsOpen { get; private set; }
        public bool IsFlightReserved { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Flight Flight { get; private set; }

        public Gate(int id)
        {
            ID = id;
            IsOpen = false;
            Luggages = new Queue<Luggage>();
        }

        internal void Open()
        {
            IsOpen = true;
        }
        internal void Close()
        {
            Luggages.Clear();
            RemoveReservedFlight();
            IsOpen = false;
        }

        internal void AddLuggage(Luggage luggage)
        {
            Luggages.Enqueue(luggage);
        }

        internal void ReserveFlight(Flight flight)
        {
            Flight = flight;
            IsFlightReserved = true;
        }
        internal void RemoveReservedFlight()
        {
            Flight = null;
            IsFlightReserved = false;
        }

        internal void ExportLuggageToFlight()
        {
            if (IsFlightReserved)
            {
                Flight.LoadLuggages(Luggages);
                Luggages = new Queue<Luggage>();
            }
        }
    }
}
