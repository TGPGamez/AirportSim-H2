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

        /// <summary>
        /// Open Gate
        /// </summary>
        internal void Open()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Close Gate
        /// </summary>
        internal void Close()
        {
            Luggages.Clear();
            RemoveReservedFlight();
            IsOpen = false;
        }

        /// <summary>
        /// Add luggage
        /// </summary>
        /// <param name="luggage"></param>
        internal void AddLuggage(Luggage luggage)
        {
            Luggages.Enqueue(luggage);
        }

        /// <summary>
        /// Reserve a Flight
        /// </summary>
        /// <param name="flight"></param>
        internal void ReserveFlight(Flight flight)
        {
            Flight = flight;
            IsFlightReserved = true;
        }

        /// <summary>
        /// Remove the reserved Flight
        /// </summary>
        internal void RemoveReservedFlight()
        {
            Flight = null;
            IsFlightReserved = false;
        }

        /// <summary>
        /// Export all luggage onboard Flight
        /// </summary>
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
