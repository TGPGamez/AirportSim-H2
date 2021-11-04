using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.LuggageSorting
{
    public class Gate
    {
        public int ID { get; private set; }
        public bool IsOpen { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }

        public Gate(int id)
        {
            ID = id;
            IsOpen = false;
            Luggages = new Queue<Luggage>();
        }

        internal void Open()
        {
            IsOpen = true;
        }

        internal void Close()
        {
            IsOpen = false;
        }
    }
}
