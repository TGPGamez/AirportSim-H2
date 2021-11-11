using AirportSim_H2.Simulation.FlightRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.BaggageSorting
{
    public class GatesInAirport
    {
        public Gate[] Gates { get; private set; }

        public GatesInAirport(int gateAmount)
        {
            Gates = new Gate[gateAmount];
            Reset();
        }

        internal void Reset()
        {
            for (int i = 0; i < Gates.Length; i++)
            {
                Gates[i] = new Gate(i);
            }
        }

        internal void OpenGatesForFlights(FlightSchedule flightSchedule)
        {
            List<Flight> flights = flightSchedule.Flights.FindAll(x => x.Status == FlightStatus.OnTheWay);
            foreach (Flight flight in flights)
            {
                if (!flight.IsAtGate)
                {
                    Gate gate = Gates.FirstOrDefault(x => !x.IsFlightReserved);
                    if (gate != null)
                    {
                        flight.AssignGate(gate);
                        gate.ReserveFlight(flight);
                    }
                }
            }
        }

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
