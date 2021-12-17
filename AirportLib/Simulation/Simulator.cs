using System.Threading;
using System;

namespace AirportLib
{
    public class Simulator
    {
        public event MessageEvent ExceptionInfo;

        public bool IsStarted { get; private set; }
        public bool IsAutoGenereatedReservationsEnabled { get; set; }
       
        private int activityLevel = 4;
        public int ActivityLevel
        {
            get { return activityLevel; }
            set 
            {
                if (value > 0 && value <= 10)
                {
                    activityLevel = value;
                }
            }
        }

        public Time Time { get; private set; }
        public CountersInAirport CountersInAirport { get; private set; }
        public GatesInAirport GatesInAirport { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }
        public Sorter Sorter { get; private set; }

        public Simulator(int counterAmount, int gateAmount, int beltLength)
        {
            Time = new Time();
            Time.TimeUpdate = OnTimeUpdate;

            CountersInAirport = new CountersInAirport(counterAmount);
            GatesInAirport = new GatesInAirport(gateAmount);
            FlightSchedule = new FlightSchedule(Time);
            Sorter = new Sorter(beltLength, CountersInAirport, GatesInAirport, Time);
        }

        /// <summary>
        /// Starts simulation
        /// </summary>
        public void Start()
        {
            if (!IsStarted)
            {
                Time.Start();
                Sorter.Start();
                Update();
                IsStarted = true;
            }
        }

        /// <summary>
        /// Stops simulation
        /// </summary>
        public void Stop()
        {
            Time.Stop();
            Sorter.Stop();
        }

        /// <summary>
        /// Restart simulation
        /// </summary>
        public void Restart()
        {
            Time.Stop();

            FlightSchedule.Clear();
            Sorter.Clear();
            CountersInAirport.Clear();
            GatesInAirport.Clear();

            Update();

            Time.Start();
        }

        /// <summary>
        /// Update Airport
        /// </summary>
        public void Update()
        {
            FlightSchedule.GenerateFlights(ActivityLevel, IsAutoGenereatedReservationsEnabled);
            FlightSchedule.UpdateFlightsStatuses();
            FlightSchedule.RemoveExpiredFlights();
            FlightSchedule.UpdateActiveFlights();

            CountersInAirport.AutoCheckIn();
            CountersInAirport.OpenCountersForFlights(FlightSchedule);
            CountersInAirport.CloseCounters();

            GatesInAirport.OpenGatesForFlights(FlightSchedule);
            GatesInAirport.CloseGates();
        }

        /// <summary>
        /// Used to update simulation
        /// Method that is called when our TimeUpdate event on our timer elapse
        /// </summary>
        private void OnTimeUpdate()
        {
            try
            {
                Monitor.Enter(this);

                Update();

                Monitor.PulseAll(this);
                Monitor.Exit(this);
            }
            catch (Exception ex)
            {
                ExceptionInfo?.Invoke("Simulation thread crashed: " + ex.Message);
                Time.Stop();
            }
        }
    }
}
