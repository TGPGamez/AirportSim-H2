using AirportSim_H2.Simulation.LuggageSorting;
using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.FlightRelated
{

    public enum FlightStatus
    {
        OpenForReservation,
        FarAway,
        OnTheWay,
        Landing,
        Refilling,
        Boarding,
        Takeoff,
        Canceled,
    }

    public class Flight
    {
        private static readonly Random rand = new Random();
        public static Flight Empty = new Flight("", 0, DateTime.MinValue, DateTime.MaxValue, "");
        public MessageEvent FlightInfo { get; set; }

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

        public bool IsReadyForCheckIn()
        {
            return Status == FlightStatus.OnTheWay || Status == FlightStatus.Landing;
        }

        public bool HaveSeatsAvaiable()
        {
            return Reservations.Count < SeatsAmount;
        }

        public int GetCheckedInPassengers()
        {
            return Reservations.FindAll(x => x.IsCheckedIn).Count;
        }

        internal void BookFlightTicket(Passenger passenger)
        {
            if (Status == FlightStatus.OpenForReservation)
            {
                Reservations.Add(new Reservation(passenger, this));
                //Log event
            }
        }

        internal void AutoBookFlightTickets(int minSeats)
        {
            if (minSeats > SeatsAmount)
            {
                minSeats = SeatsAmount - 1;
            }
            int ticketAmount = rand.Next(minSeats, SeatsAmount);
            for (int i = 0; i < ticketAmount; i++)
            {
                Passenger passenger = AutoGenerator.CreateRandomPassenger();
                BookFlightTicket(passenger);
            }
            //Log event
        }
    }
}
