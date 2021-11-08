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
        public Time Time { get; set; }
        public bool IsStarted { get; private set; }

        public MessageEvent ExceptionEvent { get; set; }

        public Simulator()
        {
            Time = new Time();
            Time.TimeUpdate = OnTimeUpdate;
        }


        public void Start()
        {
            if (!IsStarted)
            {
                Time.Start();

                Update();
                IsStarted = true;
            }
        }

        public void Update()
        {

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
                ExceptionEvent?.Invoke("Simulation thread crashed: " + ex.Message);
                Time.Stop();
            }
        }
    }
}
