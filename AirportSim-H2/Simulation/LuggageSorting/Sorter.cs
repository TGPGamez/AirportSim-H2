using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.LuggageSorting
{
    public class Sorter
    {
        private static readonly Random rand = new Random();
        private readonly Gate[] Gates;
        private readonly Counter[] Counters;
        private Thread sorterThread;

        public MessageEvent ProcessInfo { get; set; }
    }
}
