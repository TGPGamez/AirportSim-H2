namespace AirportLib
{
    public delegate void MessageEvent(string message);
    public delegate void UpdatedEvent();
    public delegate void FlightAddEvent(Flight flight);
    public delegate void FlightRemoveEvent(Flight flight);
}
