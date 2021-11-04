using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.ReservationRelated
{
    public class Passenger
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public Passenger(string firstName, string lastName, string mail, string phoneNr, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Mail = mail;
            PhoneNumber = phoneNr;
            Address = address;
        }
    }
}
