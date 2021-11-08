using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation
{
    public class Delegates
    {
        public delegate void MessageEvent(string message);
        public delegate void TimeEvent();
    }
}
