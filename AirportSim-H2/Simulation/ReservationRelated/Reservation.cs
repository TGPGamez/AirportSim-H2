namespace AirportLib
{
    public class Reservation
    {
        public Passenger Passenger { get; private set; }
        public Flight Flight { get; private set; }
        public bool IsCheckedIn { get; set; }

        public Reservation(Passenger passenger, Flight flight)
        {
            Passenger = passenger;
            Flight = flight;
            IsCheckedIn = false;
        }
    }
}
