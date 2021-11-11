using System.Collections.Generic;
using System.Linq;

namespace AirportLib
{
    public class CountersInAirport
    {
        public Counter[] Counters { get; private set; }
        public Flight Flight { get; private set; }

        public CountersInAirport(int counterAmount)
        {
            Counters = new Counter[counterAmount];
            Clear();
        }

        internal void Clear()
        {
            for (int i = 0; i < Counters.Length; i++)
            {
                Counters[i] = new Counter(i);
            }
        }

        internal void AutoCheckIn()
        {
            foreach (Counter counter in Counters)
            {
                if (counter.IsOpen && !counter.IsLuggageReady())
                {
                    Reservation reservation = counter.Flight.Reservations.Find(x => !x.IsCheckedIn);
                    if (reservation != null && reservation.Flight.Gate != null)
                    {
                        CheckInLuggage(new Luggage(reservation.Flight.Gate.ID, counter.ID, reservation));
                    }
                }
            }
        }

        internal void CheckInLuggage(Luggage luggage)
        {
            Counter counter = Counters.FirstOrDefault(x => x.ID == luggage.CounterID);
            counter.CheckInLuggage(luggage);
            luggage.Reservation.IsCheckedIn = true;
        }

        internal void OpenCountersForFlights(FlightSchedule flightSchedule)
        {
            List<Flight> flights = flightSchedule.Flights.FindAll(x => x.CanCheckIn() == true);
            foreach(Flight flight in flights)
            {
                Counter counter = Counters.Where(x => x.IsOpen == false).FirstOrDefault();
                if (Counters.Any(x => x.Flight == flight))
                {
                    continue;
                }
                if (counter != null)
                {
                    counter.AssignFlight(flight);
                    counter.Open();
                    continue;
                }
            }
        }

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
