using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2
{
    public static class Data
    {
        private static Random rand = new Random();

        public static readonly string[] CityDestinations;
        public static readonly string[] Names;
        public static readonly string[] Streets;

        static Data()
        {
            CityDestinations = File.ReadLines(@"..\..\..\Resources\Cities.txt").ToArray();
            Names = File.ReadLines(@"..\..\..\Resources\Names.txt").ToArray();
            Streets = File.ReadLines(@"..\..\..\Resources\Streets.txt").ToArray();
        }

        public static string GetRandomCity()
        {
            return CityDestinations[rand.Next(0, CityDestinations.Length)];
        }
        public static string GetRandomName()
        {
            return Names[rand.Next(0, Names.Length)];
        }
        public static string GetRandomStreet()
        {
            return Streets[rand.Next(0, Streets.Length)];
        }
    }
}
