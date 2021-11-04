using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation
{
    public enum FlightStatus
    {
        OpenForReservation,
        FarAway,
        OnTheWay,
        Landing,
        Refilling,
        Boarding,
        Takeoff,
        Canceled,
    }
}
