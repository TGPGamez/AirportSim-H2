using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AirportSim_H2.Simulation.Delegates;

namespace AirportSim_H2.Simulation
{
    public class Time
    {
        private readonly System.Timers.Timer Timer;
        internal TimeEvent TimeUpdate { get; set; }
        public DateTime DateTime { get; private set; }
        private bool isPausedRequested = false;
        private int speed;

        public int Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                Timer.Interval = 1000 / speed;
            }
        }

        public Time()
        {
            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += OnTick;

            DateTime = DateTime.Now;
            Speed = 1;
        }

        internal void Start()
        {
            Timer.Enabled = true;
        }

        internal void Stop()
        {
            isPausedRequested = true;
            while (Timer.Enabled)
            {
                Thread.Sleep(1);
            }
            DateTime = DateTime.Now;
        }

        private void OnTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this))
            {
                TimeUpdate.Invoke();
                DateTime = DateTime.AddSeconds(15);
                Monitor.PulseAll(this);
                Monitor.Exit(this);
            }
            if (isPausedRequested)
            {
                Timer.Enabled = false;
                isPausedRequested = false;
            }
        }
    }
}
