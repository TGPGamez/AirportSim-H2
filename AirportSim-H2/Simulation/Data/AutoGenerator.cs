using AirportSim_H2.Simulation.ReservationRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation
{
    public static class AutoGenerator
    {
        private static Random rand = new Random();

        public static Passenger CreateRandomPassenger()
        {
            string firstName = Data.GetRandomName();

            return new Passenger(
                firstName,
                Data.GetRandomName(),
                firstName + rand.Next(1, 10000) + "@gmail.com",
                "+45" + rand.Next(10000000, 99999999),
                $"{Data.GetRandomCity()} {Data.GetRandomStreet()} {rand.Next(1, 1000)}"
            );
        }
    }
}
