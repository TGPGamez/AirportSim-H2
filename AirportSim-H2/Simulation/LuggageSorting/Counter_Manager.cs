using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.LuggageSorting
{
    public class Counter_Manager
    {
        public Counter[] Counters { get; private set; }

        public Counter_Manager(int counterAmount)
        {
            Counters = new Counter[counterAmount];
            Reset();
        }

        internal void Reset()
        {
            for (int i = 0; i < Counters.Length; i++)
            {
                Counters[i] = new Counter(i);
            }
        }
    }
}
