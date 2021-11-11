using AirportSim_H2.Simulation.BaggageSorting;
using AirportSim_H2.Simulation.FlightRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AirportSim_H2.Simulation.Delegates;

namespace AirportSim_H2.Simulation
{
    public class Simulator
    {
        public MessageEvent ExceptionInfo { get; set; }

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

        public Simulator(int counterAmount, int gateAmount, int flightDisplayLength, int beltLength)
        {
            Time = new Time();
            Time.TimeUpdate = OnTimeUpdate;

            CountersInAirport = new CountersInAirport(counterAmount);
            GatesInAirport = new GatesInAirport(gateAmount);
            FlightSchedule = new FlightSchedule(Time, flightDisplayLength);
            Sorter = new Sorter(beltLength, CountersInAirport, GatesInAirport, Time);
        }


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

        public void Update()
        {
            FlightSchedule.GenerateFlights(ActivityLevel, IsAutoGenereatedReservationsEnabled);
            FlightSchedule.UpdateFlightsStatuses();
            FlightSchedule.RemoveExpiredFlights();
            FlightSchedule.UpdateFlightDisplay();

            CountersInAirport.AutoCheckIn();
            CountersInAirport.OpenCountersForFlights(FlightSchedule);
            CountersInAirport.CloseCounters();

            GatesInAirport.OpenGatesForFlights(FlightSchedule);
            GatesInAirport.CloseGates();
        }

        public void Restart()
        {
            Time.Stop();
            Update();
            Time.Start();
        }

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
