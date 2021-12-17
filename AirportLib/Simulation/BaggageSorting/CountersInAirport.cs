using System.Collections.Generic;
using System.Linq;

namespace AirportLib
{
    public class CountersInAirport
    {
        public Counter[] Counters { get; private set; }

        public CountersInAirport(int counterAmount)
        {
            Counters = new Counter[counterAmount];
            Clear();
        }

        /// <summary>
        /// Clears all Counter's
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Counters.Length; i++)
            {
                Counters[i] = new Counter(i);
            }
        }

        /// <summary>
        /// Auto checks in luggage for each Counter
        /// </summary>
        internal void AutoCheckIn()
        {
            foreach (Counter counter in Counters)
            {
                //Checks if counter is open and doesn't have luggage
                if (counter.IsOpen && !counter.IsLuggageReady())
                {
                    //Finds a reservation from Flight  where the reservation isn't checked in
                    Reservation reservation = counter.Flight.Reservations.Find(x => !x.IsCheckedIn);
                    if (reservation != null && reservation.Flight.Gate != null)
                    {
                        //Chekcs in luggage
                        CheckInLuggage(new Luggage(reservation.Flight.Gate.ID, counter.ID, reservation));
                    }
                }
            }
        }

        /// <summary>
        /// Checks in a luggage
        /// </summary>
        /// <param name="luggage"></param>
        internal void CheckInLuggage(Luggage luggage)
        {
            //Finds first available counter with CounterID from luggage
            Counter counter = Counters.FirstOrDefault(x => x.ID == luggage.CounterID);
            counter.CheckInLuggage(luggage);
            luggage.Reservation.IsCheckedIn = true;
        }

        /// <summary>
        /// Opens Counters for Flights out from FlightSchedule
        /// </summary>
        /// <param name="flightSchedule"></param>
        internal void OpenCountersForFlights(FlightSchedule flightSchedule)
        {
            //Finds all flights where checkin is open
            List<Flight> flights = flightSchedule.Flights.FindAll(x => x.CanCheckIn() == true);
            foreach(Flight flight in flights)
            {
                //Finds first counter that is open
                Counter counter = Counters.Where(x => x.IsOpen == false).FirstOrDefault();
                //Checks if found counter has a flight assigned
                if (Counters.Any(x => x.Flight == flight))
                {
                    continue;
                }
                if (counter != null)
                {
                    //Assigns flight and opens counter
                    counter.ReserveFlight(flight);
                    counter.Open();
                    continue;
                }
            }
        }

        /// <summary>
        /// Closes Counters
        /// </summary>
        internal void CloseCounters()
        {
            foreach (Counter counter in Counters)
            {
                Flight flight = counter.Flight;
                if (!flight.CanCheckIn() || flight.GetCheckedInAmount() == flight.Reservations.Count)
                {
                    counter.Close();
                }
            }
        }
    }
}
