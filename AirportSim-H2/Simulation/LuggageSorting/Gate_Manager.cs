using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.LuggageSorting
{
    public class Gate_Manager
    {
        public Gate[] Gates { get; private set; }

        public Gate_Manager(int gateAmount)
        {
            Gates = new Gate[gateAmount];
            Reset();
        }

        internal void Reset()
        {
            for (int i = 0; i < Gates.Length; i++)
            {
                Gates[i] = new Gate(i);
            }
        }
    }
}
