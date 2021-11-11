namespace AirportLib
{
    public class Luggage
    {
        public int GateID { get; set; }
        public int CounterID { get; set; }
        public Reservation Reservation { get; set; }

        public Luggage(int gateID, int counterID, Reservation reservation)
        {
            GateID = gateID;
            CounterID = counterID;
            Reservation = reservation;
        }
    }
}
