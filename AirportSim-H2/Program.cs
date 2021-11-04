using System;

namespace AirportSim_H2
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(Data.GetRandomCity());
            }
            Console.ReadKey();
        }
    }
}
