namespace AirportLib
{
    public class Counter
    {
        public int ID { get; private set; }
        public Luggage Luggage { get; private set; }
        public Flight Flight { get; private set; }
        public bool IsOpen { get; private set; }

        public event MessageEvent CounterExceptionInfo;

        public Counter(int id)
        {
            ID = id;
            Luggage = null;
            Flight = Flight.Empty;
            IsOpen = false;
        }

        internal void Open()
        {
            IsOpen = true;
        }

        internal void Close()
        {
            IsOpen = false;
            Flight = Flight.Empty;
        }


        internal void CheckInLuggage(Luggage luggage)
        {
            if (Luggage == null)
            {
                Luggage = luggage;
            } else
            {
                CounterExceptionInfo?.Invoke($"There is already luggage on counter {ID}");
            }
        }

        internal void AssignFlight(Flight flight)
        {
            Flight = flight;
        }

        internal Luggage GetLuggageFromCounter()
        {
            Luggage luggage = Luggage;
            Luggage = null;
            return luggage;
        }

        public bool IsLuggageReady()
        {
            return (IsOpen) && (Luggage != null);
        }
    }
}
