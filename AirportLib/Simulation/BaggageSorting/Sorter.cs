using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AirportLib
{
    public class Sorter
    {
        private static readonly Random rand = new Random();
        public event MessageEvent SortingInfo;
        public event MessageEvent SortingExceptionInfo;

        private readonly Time time;
        private readonly Counter[] counters;
        private readonly Gate[] gates;
        private bool isClearRequested = false;
        private bool isStopRequested = false;

        private Thread sorterThread;
        public Belt<Luggage> Belt { get; private set; }

        public Sorter(int beltLength, CountersInAirport countersInAirport, GatesInAirport gatesInAirport, Time time)
        {
            Belt = new Belt<Luggage>(beltLength);
            counters = countersInAirport.Counters;
            gates = gatesInAirport.Gates;
            this.time = time;
        }

        internal void Start()
        {
            sorterThread = new Thread(SortingProcess);
            sorterThread.Name ="AP-Sorter";
            sorterThread.Start();
        }

        internal void Clear()
        {
            isClearRequested = true;
        }

        internal void Stop()
        {
            isStopRequested = true;
            sorterThread.Join();
        }

        private void SortingProcess()
        {
            try
            {
                while (true)
                {
                    CounterToSorterProcess();
                    SorterToGateProcess();
                    Thread.Sleep(128 / time.Speed);
                    if (isClearRequested)
                    {
                        Belt.Clear();
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
                List<Counter> counters = this.counters.Where(x => x.IsLuggageReady()).ToList();
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
                Gate gate = gates.FirstOrDefault(x => x.ID == luggage.GateID);
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
