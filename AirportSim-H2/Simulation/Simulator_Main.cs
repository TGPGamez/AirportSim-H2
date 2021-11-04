using AirportSim_H2.Simulation.LuggageSorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation
{
    public partial class Simulator_Main
    {
        public Gate_Manager GateManager { get; private set; }
        public Counter_Manager CounterManager { get; private set; }
        public bool IsSimulationStarted { get; private set; }

        public Simulator_Main(int gateAmount, int counterAmount)
        {
            GateManager = new Gate_Manager(gateAmount);
            CounterManager = new Counter_Manager(counterAmount);
        }


        public void Start()
        {
            if (!IsSimulationStarted)
            {
                //Start

                IsSimulationStarted = true;
            }
        }
    }
}
