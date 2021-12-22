using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportLib
{
    public class Flight : ILog
    {
        public event MessageEvent FlightInfo;
        public event MessageEvent FlightExceptionInfo;

        private static readonly Random rand = new Random();
        public static Flight Empty = new Flight("", 0, "", DateTime.MinValue, DateTime.MinValue);

        public string Name { get; private set; }
        public int SeatsAmount { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Gate Gate { get; private set; }
        public bool IsAtGate { get; private set; }

        public Flight(string name, int seatsAmount, string destination, DateTime departure, DateTime arrival)
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

        /// <summary>
        /// Gets amount of reservations thats checked in
        /// </summary>
        /// <returns></returns>
        public int GetCheckedInAmount()
        {
            return Reservations.FindAll(x => x.IsCheckedIn).Count;
        }

        /// <summary>
        /// Gets the amount of reservations that isn't checked in
        /// </summary>
        /// <returns></returns>
        public int GetNoCheckedInAmount()
        {
            return Reservations.Count - GetCheckedInAmount();
        }

        /// <summary>
        /// Check if it is possible to check in to flight
        /// </summary>
        /// <returns></returns>
        public bool CanCheckIn()
        {
            return Status == FlightStatus.OnTheWay || Status == FlightStatus.Landing;
        } 

        /// <summary>
        /// Load luggages onboard flight
        /// </summary>
        /// <param name="luggages"></param>
        internal void LoadLuggages(Queue<Luggage> luggages)
        {
            Luggages = luggages;
        }

        /// <summary>
        /// Reserve the flight to a gate
        /// </summary>
        /// <param name="gate"></param>
        internal void ReserveGate(Gate gate)
        {
            Gate = gate;
            IsAtGate = true;
        }

        /// <summary>
        /// Book a ticket to the flight
        /// </summary>
        /// <param name="passenger"></param>
        internal void BookFlightTicket(Passenger passenger)
        {
            if (Status == FlightStatus.OpenForReservation)
            {
                Reservations.Add(new Reservation(passenger, this));
                //Gets the message out from our custom Attribute LogMessage that is on our FlightStatus
                //and replaces placeholder values with information values
                string logMessage = FlightStatus.OpenForReservation.GetAttribute<LogMessage>()
                    .Message.ReplaceWithValue("firstName|destination", $"{passenger.FirstName}|{Destination}");
                Log(logMessage);
            } else
            {
                //Log failed reservation due to flight is full
                Log("Reservation failed, flight is full.");
            }
        }

        /// <summary>
        /// Auto book flight tickets
        /// </summary>
        /// <param name="minSeats"></param>
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
            Log($"{amount} auto generated people has made a reservation on {Name}");
        }

        internal void UpdateStatus(Time time)
        {
            string logMessage;
            if (Status != FlightStatus.OpenForReservation && Reservations.Count < 20)
            {
                if (ChangeStatusIfNew(FlightStatus.Canceled))
                {
                    //Gets the message out from our custom Attribute LogMessage that is on our FlightStatus
                    //and replaces placeholder values with information values
                    logMessage = FlightStatus.Canceled.GetAttribute<LogMessage>().Message.ReplaceWithValue("name", Name);
                    ExceptionLog(logMessage);
                    //FlightExceptionInfo?.Invoke(logMessage);
                }
            } else
            {
                double timeToTakeOff = Departure.Subtract(time.DateTime).TotalMinutes;
                logMessage = ChangeStatusesInsidePeriods(timeToTakeOff);
                Log(logMessage);
            }

        }

        //Used to get values from FlightStatus except for Canceled and OpenForReservation
        private static IEnumerable<FlightStatus> statuses = Enum
                .GetValues(typeof(FlightStatus))
                .Cast<FlightStatus>()
                .Where(item => item != FlightStatus.Canceled || item != FlightStatus.OpenForReservation || item != FlightStatus.TakeoffMissing);

        /// <summary>
        /// Used to changes status out from the time to take off and return LogMessage
        /// </summary>
        /// <param name="timeToTakeOff"></param>
        /// <returns></returns>
        private string ChangeStatusesInsidePeriods(double timeToTakeOff)
        {
            string logMessage = string.Empty;
            foreach (FlightStatus status in statuses)
            {
                //Gets min and max period from StatusField Attribute
                int minPeriod = status.GetAttribute<StatusField>().Minperiod;
                int maxPeriod = status.GetAttribute<StatusField>().Maxperiod;
                //Determines if the timeToTakeOff is between our min and max period
                if ((timeToTakeOff > minPeriod) && (timeToTakeOff < maxPeriod))
                {
                    if (ChangeStatusIfNew(status))
                    {
                        if (status == FlightStatus.Takeoff)
                        {
                            if (GetCheckedInAmount() == Reservations.Count)
                            {
                                logMessage = FlightStatus.TakeoffMissing.GetAttribute<LogMessage>().Message;
                                continue;
                            }
                        }
                        //Gets the message out from our custom Attribute LogMessage that is on our FlightStatus
                        logMessage = status.GetAttribute<LogMessage>().Message;
                    }
                }
            }
            //Replaces placeholder values with information values
            return logMessage.ReplaceWithValue("name", Name);
        }

        /// <summary>
        /// Changes FlightStatus if it isn't the same FlightStatus
        /// </summary>
        /// <param name="flightStatus"></param>
        /// <returns></returns>
        private bool ChangeStatusIfNew(FlightStatus flightStatus)
        {
            if (Status != flightStatus)
            {
                Status = flightStatus;
                return true;
            }
            return false;
        }

        public void Log(string message)
        {
            if (message.Length > 0)
            {
                FlightInfo?.Invoke(message);
            }
        }

        public void ExceptionLog(string message)
        {
            if (message.Length > 0)
            {
                FlightExceptionInfo?.Invoke(message);
            }
        }
    }
}
