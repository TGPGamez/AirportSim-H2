using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AirportSim_H2.Simulation.Delegates;

namespace AirportSim_H2.Simulation.BaggageSorting
{
    public class Sorter
    {
        private static readonly Random rand = new Random();
        public MessageEvent SortingInfo { get; set; }
        public MessageEvent SortingExceptionInfo { get; set; }

        private readonly Time Time;
        private readonly Counter[] Counters;
        private readonly Gate[] Gates;
        private bool isClearRequested = false;

        public Belt<Luggage> Belt { get; private set; }

        public Sorter(int beltLength, CountersInAirport countersInAirport, GatesInAirport gatesInAirport, Time time)
        {
            Belt = new Belt<Luggage>(beltLength);
            Counters = countersInAirport.Counters;
            Gates = gatesInAirport.Gates;
            Time = time;
        }

        internal void Start()
        {
            Thread sorterThread = new Thread(SortingProcess);
            sorterThread.Start();
        }

        internal void Clear()
        {
            isClearRequested = true;
        }

        private void SortingProcess()
        {
            try
            {
                while (true)
                {
                    CounterToSorterProcess();
                    SorterToGateProcess();
                    Thread.Sleep(128 / Time.Speed);
                    if (isClearRequested)
                    {
                        Belt.Reset();
                        isClearRequested = false;
                    }
                }
            }
            catch (Exception ex)
            {
                SortingExceptionInfo?.Invoke("Thread SorterThread crashed: " + ex.Message);
            }
        }

        private void CounterToSorterProcess()
        {
            if (Belt.IsSpace())
            {
                List<Counter> counters = Counters.Where(x => x.IsLuggageReady()).ToList();
                if (counters.Count != 0)
                {
                    Counter counter = counters[rand.Next(0, counters.Count)];

                    Luggage luggage = counter.GetLuggageFromCounter();

                    Belt.Push(luggage);

                    SortingInfo?.Invoke($"Luggage owned by {luggage.Reservation.Passenger.FirstName} is now on its way to gate {luggage.GateID}");
                }
            }
        }

        private void SorterToGateProcess()
        {
            if (!Belt.IsPullEmpty())
            {
                Luggage luggage = Belt.Pull();
                Gate gate = Gates.FirstOrDefault(x => x.ID == luggage.GateID);
                if (gate != null)
                {
                    gate.AddLuggage(luggage);
                }
                SortingInfo?.Invoke($"Luggaged owned by {luggage.Reservation.Passenger.FirstName} is now at gate {gate.ID}");
            } else
            {
                Belt.MoveForward();
            }
        }
    }
}
