using System.Collections.Generic;
using System.Linq;

namespace AirportLib
{
    public class GatesInAirport
    {
        public Gate[] Gates { get; private set; }

        public GatesInAirport(int gateAmount)
        {
            Gates = new Gate[gateAmount];
            for (int i = 0; i < Gates.Length; i++)
            {
                Gates[i] = new Gate(i);
            }
        }

        /// <summary>
        /// Clear Gates
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Gates.Length; i++)
            {
                Gates[i].Close();
            }
        }

        /// <summary>
        /// Open all gates for Flights
        /// </summary>
        /// <param name="flightSchedule"></param>
        internal void OpenGatesForFlights(FlightSchedule flightSchedule)
        {
            //Finds all flights where FlightStatus is OnTheWay
            List<Flight> flights = flightSchedule.Flights.FindAll(x => x.Status == FlightStatus.OnTheWay);
            foreach (Flight flight in flights)
            {
                //Check if flight isn't reserved to gate
                if (!flight.IsAtGate)
                {
                    //Find first gate that hasn't a flight reserved
                    Gate gate = Gates.FirstOrDefault(x => !x.IsFlightReserved);
                    if (gate != null)
                    {
                        flight.ReserveGate(gate);
                        gate.ReserveFlight(flight);
                    }
                }
            }
        }

        /// <summary>
        /// Closes Gates
        /// </summary>
        internal void CloseGates()
        {
            foreach (Gate gate in Gates)
            {
                if (gate.IsFlightReserved)
                {
                    if (gate.Flight.Status == FlightStatus.Takeoff)
                    {
                        gate.RemoveReservedFlight();
                        gate.Close();
                    }
                    else if (gate.Flight.Status == FlightStatus.Refilling && gate.Luggages.Count > 0)
                    {
                        gate.ExportLuggageToFlight();
                    }
                }
            }
        }
    }
}
