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

        /// <summary>
        /// Starts the Sorting
        /// </summary>
        internal void Start()
        {
            sorterThread = new Thread(SortingProcess);
            sorterThread.Name ="AP-Sorter";
            sorterThread.Start();
        }

        /// <summary>
        /// Request a Clear
        /// </summary>
        internal void Clear()
        {
            isClearRequested = true;
        }

        /// <summary>
        /// Stop Sorting
        /// </summary>
        internal void Stop()
        {
            isStopRequested = true;
            sorterThread.Join();
        }

        /// <summary>
        /// The process for sorting luggage
        /// </summary>
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

        /// <summary>
        /// Process to sort from Counter to Sorter
        /// </summary>
        private void CounterToSorterProcess()
        {
            if (Belt.IsSpace())
            {
                //Get all Counter where luggage is ready/set
                List<Counter> counters = this.counters.Where(x => x.IsLuggageReady()).ToList();
                if (counters.Count != 0)
                {
                    //Get random Counter
                    Counter counter = counters[rand.Next(0, counters.Count)];

                    //Get Luggage from Counter
                    Luggage luggage = counter.GetLuggageFromCounter();

                    //"Puts luggage on belt"
                    Belt.Push(luggage);

                    SortingInfo?.Invoke($"Luggage owned by {luggage.Reservation.Passenger.FirstName} is now on its way to gate {luggage.GateID}");
                }
            }
        }

        /// <summary>
        /// Process to sort from Sorter to Gate
        /// </summary>
        private void SorterToGateProcess()
        {
            //Check if last index of Belt is set
            if (!Belt.IsPullEmpty())
            {
                //Get the Luggage from belt
                Luggage luggage = Belt.Pull();
                
                //Find first Gate with same ID as luggage's GateID
                Gate gate = gates.FirstOrDefault(x => x.ID == luggage.GateID);
                if (gate != null)
                {
                    gate.AddLuggage(luggage);
                }
                SortingInfo?.Invoke($"Luggaged owned by {luggage.Reservation.Passenger.FirstName} is now at gate {gate.ID}");
            } else
            {
                //Move all luggages
                Belt.MoveForward();
            }
        }
    }
}
