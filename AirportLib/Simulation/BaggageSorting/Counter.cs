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

        /// <summary>
        /// Open Counter
        /// </summary>
        internal void Open()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Close counter
        /// </summary>
        internal void Close()
        {
            IsOpen = false;
            Flight = Flight.Empty;
        }

        /// <summary>
        /// Checkin a luggage
        /// </summary>
        /// <param name="luggage"></param>
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

        /// <summary>
        /// Reserve Flight to Counter
        /// </summary>
        /// <param name="flight"></param>
        internal void ReserveFlight(Flight flight)
        {
            Flight = flight;
        }

        /// <summary>
        /// Get the Luggage from the Counter
        /// Method is used when moving luggage
        /// </summary>
        /// <returns>Luggage</returns>
        internal Luggage GetLuggageFromCounter()
        {
            Luggage luggage = Luggage;
            Luggage = null;
            return luggage;
        }

        /// <summary>
        /// Check if Gate is open and has Luggage
        /// </summary>
        /// <returns></returns>
        public bool IsLuggageReady()
        {
            return (IsOpen) && (Luggage != null);
        }
    }
}
