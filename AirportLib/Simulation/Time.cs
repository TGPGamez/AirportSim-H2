using System;
using System.Threading;

namespace AirportLib
{
    public class Time
    {
        private readonly System.Timers.Timer Timer;
        internal UpdatedEvent TimeUpdate { get; set; }
        public DateTime DateTime { get; private set; }
        
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

        /// <summary>
        /// Starts Timer
        /// </summary>
        internal void Start()
        {
            Timer.Enabled = true;
        }

        /// <summary>
        /// Stops Timer
        /// </summary>
        internal void Stop()
        {
            Timer.Enabled = false;
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// Event to call when Timer is Elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this))
            {
                //Invoke event
                TimeUpdate.Invoke();
                //Add to "time"
                DateTime = DateTime.AddSeconds(15);
                Monitor.PulseAll(this);
                Monitor.Exit(this);
            }
        }
    }
}
