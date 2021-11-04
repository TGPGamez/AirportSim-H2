using AirportSim_H2.Simulation.LuggageSorting;
using AirportSim_H2.Simulation.Reservation;
using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.FlightRelated
{
    public class Flight
    {
        public string Name { get; private set; }
        public int SeatsAmount { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Gate Gate { get; private set; }
        public bool IsAtTerminal { get; private set; }

        public Flight(string name, int seatsAmount, DateTime arrival, DateTime departure, string destination)
        {
            Name = name;
            Gate = null;
            Arrival = arrival;
            Departure = departure;
            Destination = destination;
            Status = FlightStatus.OpenForReservation;
            SeatsAmount = seatsAmount;
            Reservations = new List<Reservation>();
            Luggages = new Queue<Luggage>();
        }
    }
}
