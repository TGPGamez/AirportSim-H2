using AirportSim_H2.Simulation.BaggageSorting;
using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AirportSim_H2.Simulation.Delegates;

namespace AirportSim_H2.Simulation.FlightRelated
{
    public class Flight
    {
        public MessageEvent FlightInfo { get; set; }
        public MessageEvent FlightExceptionInfo { get; set; }

        private static readonly Random rand = new Random();
        public static Flight Empty = new Flight("", 0, "", DateTime.MinValue, DateTime.MinValue);

        public string Name { get; private set; }
        public int SeatsAmount { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public Gate Gate { get; private set; }
        public bool IsAtGate { get; private set; }
        public DateTime Departure { get; private set; }
        public DateTime Arrival { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public List<Reservation> Reservations { get; private set; }

        public Flight(string name, int seatsAmount, string destination, DateTime departure, DateTime arrival)
        {
            Name = name;
            SeatsAmount = seatsAmount;
            Destination = destination;
            Departure = departure;
            Arrival = arrival;
            Status = FlightStatus.OpenForReservation;
            Luggages = new Queue<Luggage>();
            Reservations = new List<Reservation>();
        }

        public int GetCheckedInAmount()
        {
            return Reservations.FindAll(x => x.IsCheckedIn).Count;
        }

        public bool CanCheckIn()
        {
            return Status == FlightStatus.OnTheWay || Status == FlightStatus.Landing;
        } 

        internal void LoadLuggages(Queue<Luggage> luggages)
        {
            Luggages = luggages;
        }

        internal void AssignGate(Gate gate)
        {
            Gate = gate;
            IsAtGate = true;
        }

        internal void BookFlightTicket(Passenger passenger)
        {
            if (Status == FlightStatus.OpenForReservation)
            {
                Reservations.Add(new Reservation(passenger, this));
                FlightInfo?.Invoke($"{passenger.FirstName} has booked a ticket to {Destination}");
            } else
            {
                //Log failed reservation due to flight is full
            }
        }

        internal void AutoBookFlightTickets(int minSeats)
        {
            if (minSeats > SeatsAmount)
            {
                minSeats = SeatsAmount - 1;
            }
            int amount = rand.Next(minSeats, SeatsAmount);
            for (int i = 0; i < amount; i++)
            {
                BookFlightTicket(AutoGenerator.CreateRandomPassenger());
            }
            FlightInfo?.Invoke($"{amount} auto generated people has made a reservation on {Name}");
        }

        internal void UpdateStatus(Time time)
        {
            if (Status != FlightStatus.OpenForReservation && Reservations.Count < 20)
            {
                if (ChangeStatusIfNew(FlightStatus.Canceled))
                {
                    FlightExceptionInfo?.Invoke($"{Name} got cancelled due to insufficient reservations");
                }
            }

            double timeToTakeOff = Departure.Subtract(time.DateTime).TotalMinutes;
            // Updates the different flight states
            if (ChangeStatusInsidePeriod(timeToTakeOff, 360, 900, FlightStatus.FarAway))
            {
                FlightInfo?.Invoke($"{Name} is now closed for reservations");
            }
            if (ChangeStatusInsidePeriod(timeToTakeOff, 70, 360, FlightStatus.OnTheWay))
            {
                FlightInfo?.Invoke($"{Name} is 290 min from the airport");
            }
            if (ChangeStatusInsidePeriod(timeToTakeOff, 60, 70, FlightStatus.Landing))
            {
                FlightInfo?.Invoke($"{Name} has just landed");
            }
            if (ChangeStatusInsidePeriod(timeToTakeOff, 30, 60, FlightStatus.Refilling))
            {
                FlightInfo?.Invoke($"{Name} is being filled with luggages");
            }
            if (ChangeStatusInsidePeriod(timeToTakeOff, 5, 30, FlightStatus.Boarding))
            {
                FlightInfo?.Invoke($"{Name} is now boading");
            }
            if (ChangeStatusInsidePeriod(timeToTakeOff, 0, 5, FlightStatus.Takeoff))
            {
                if (GetCheckedInAmount() == Reservations.Count)
                {
                    FlightInfo?.Invoke($"{Name} is about to takeoff with all booked passengers");
                }
                else
                {
                    FlightExceptionInfo?.Invoke($"{Name} is about to takeoff with missing passengers due to bustle");
                }
            }

            //if (ChangeStatusesInsidePeriods(timeToTakeOff))
            //{
            //    LogUpdatedFlightStatus();
            //}

        }

        private bool ChangeStatusesInsidePeriods(double timeToTakeOff)
        {
            if (Status == FlightStatus.Canceled)
            {
                return false;
            }
            foreach (FlightStatus status in Enum.GetValues(typeof(FlightStatus)))
            {
                int minPeriod = status.GetAttribute<StatusField>().Minperiod;
                int maxPeriod = status.GetAttribute<StatusField>().Maxperiod;
                if ((timeToTakeOff > minPeriod) && (timeToTakeOff < maxPeriod))
                {
                    if (ChangeStatusIfNew(status))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ChangeStatusInsidePeriod(double timeToTakeoff, int minPeriod, int maxPeriod, FlightStatus flightStatus)
        {
            if (Status == FlightStatus.Canceled)
            {
                return false;
            }

            if ((timeToTakeoff > minPeriod) && (timeToTakeoff < maxPeriod))
            {
                return ChangeStatusIfNew(flightStatus);
            }
            return false;
        }

        private void LogUpdatedFlightStatus()
        {
            switch (Status)
            {
                case FlightStatus.FarAway:
                    FlightInfo?.Invoke($"{Name} is now closed for reservations.");
                    break;
                case FlightStatus.OnTheWay:
                    FlightInfo?.Invoke($"{Name} is 290 minutes from landing.");
                    break;
                case FlightStatus.Landing:
                    FlightInfo?.Invoke($"{Name} has just landed.");
                    break;
                case FlightStatus.Refilling:
                    FlightInfo?.Invoke($"{Name} is being filled with luggages");
                    break;
                case FlightStatus.Boarding:
                    FlightInfo?.Invoke($"{Name} is now boarding.");
                    break;
                case FlightStatus.Takeoff:
                    if (GetCheckedInAmount() == Reservations.Count)
                    {
                        FlightInfo?.Invoke($"{Name} is about to takeoff full.");
                    } else
                    {
                        FlightExceptionInfo?.Invoke($"{Name} is about to takeoff with missing passengers.");
                    }
                    break;
                default:
                    break;
            }
        }

        private bool ChangeStatusIfNew(FlightStatus flightStatus)
        {
            if (Status != flightStatus)
            {
                Status = flightStatus;
                return true;
            }
            return false;
        }
    }
}
