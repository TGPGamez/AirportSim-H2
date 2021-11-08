using AirportSim_H2.Simulation;
using System;
using System.Threading;

namespace AirportSim_H2
{
    class Program
    {
        static void GeneralInfo(string message) { Console.WriteLine(message); }
        static void SortingInfo(string message) { Console.WriteLine(message); }

        public static Simulator Simulator { get; private set; }

        static void Main(string[] args)
        {
            Simulator = new Simulator();
            Simulator.Start();
            //Simulator.Time.Speed = (int)Math.Pow(2, 5);
            while (true)
            {
                if (Monitor.TryEnter(Simulator))
                {              
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Speed: " + Simulator.Time.Speed);
                    Console.WriteLine("Time: " + Simulator.Time.DateTime.ToShortTimeString());

                    Monitor.PulseAll(Simulator);
                    Monitor.Exit(Simulator);
                }
                Thread.Sleep(10);
            }
        }
    }
}
