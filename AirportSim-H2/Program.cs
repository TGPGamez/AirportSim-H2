using AirportSim_H2.Simulation;
using AirportSim_H2.Simulation.FlightRelated;
using System;
using System.Threading;

namespace AirportSim_H2
{
    class Program
    {
        static void GeneralInfo(string message)
        {
            //Console.WriteLine(message);
        }
        static void GeneralErrorInfo(string message)
        {
            //Console.WriteLine(message);
        }
        static void FlightScheduleInfo(string message)
        {
            //Console.WriteLine(message);
        }
        static void FlightScheduleExceptionInfo(string message)
        {
            //Console.WriteLine(message);
        }
        static void SorterInfo(string message)
        {
            //Console.WriteLine(message);
        }
        static void SorterExceptionInfo(string message)
        {
            //Console.WriteLine(message);
        }


        public static Simulator Simulator { get; private set; }

        static void Main(string[] args)
        {
            Simulator = new Simulator(10, 15, 12, 20);
            Simulator.IsAutoGenereatedReservationsEnabled = true;
            Simulator.ExceptionInfo += GeneralErrorInfo;
            Simulator.Sorter.SortingExceptionInfo += SorterExceptionInfo;
            Simulator.Sorter.SortingInfo += SorterInfo;
            Simulator.FlightSchedule.FlightInfo += FlightScheduleInfo;
            Simulator.FlightSchedule.FlightExceptionInfo += FlightScheduleExceptionInfo;
            //Simulator.Time.Speed = (int)Math.Pow(2, 1);
            Simulator.Start();

            Thread display = new Thread(Display);
            Thread keyInput = new Thread(KeyInput);
            display.Start();
            keyInput.Start();
        }

        private static void Display()
        {
            while (true)
            {
                if (Monitor.TryEnter(Simulator))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"TIME: {Simulator.Time.DateTime.ToShortTimeString()}");
                    Console.WriteLine($"SPEED: {Simulator.Time.Speed}x\n");
                    Console.WriteLine("FLIGHT SCHEDULE");
                    Console.WriteLine("DEPARTURE      DESTINATION         FLIGHT      GATE    STATUS                  CHECKIN/BOOKED/MAX");
                    foreach (Flight flight in Simulator.FlightSchedule.FlightDisplay)
                    {
                        Console.Write($"{flight.Departure.ToString("HH:mm")}");
                        Console.SetCursorPosition(14, Console.CursorTop);
                        Console.Write($"{flight.Destination}");
                        Console.SetCursorPosition(34, Console.CursorTop);
                        Console.Write($"{flight.Name}");
                        Console.SetCursorPosition(46, Console.CursorTop);
                        if (flight.IsAtGate)
                        {
                            Console.Write(flight.Gate.ID.ToString());
                        }
                        Console.SetCursorPosition(54, Console.CursorTop);
                        Console.Write($"{flight.Status}");
                        Console.SetCursorPosition(78, Console.CursorTop);
                        Console.Write($"{flight.GetCheckedInAmount()}/{flight.Reservations.Count}/{flight.SeatsAmount}\n");
                    }

                    Monitor.PulseAll(Simulator);
                    Monitor.Exit(Simulator);
                }
                Thread.Sleep(10);
            }
        }

        private static void KeyInput()
        {
            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                for (int i = 0; i < 8; i++)
                {
                    if ((i + 49) == (int)key.Key)
                    {
                        Simulator.Time.Speed = (int)Math.Pow(2, i);
                    }
                }
            }
        }

    }
}
