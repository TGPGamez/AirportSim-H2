using System;

namespace AirportLib
{
    public enum FlightStatus
    {
        [StatusField(0, 0)]
        OpenForReservation,
        [StatusField(360, 900)]
        FarAway,
        [StatusField(70, 360)]
        OnTheWay,
        [StatusField(60, 70)]
        Landing,
        [StatusField(30, 60)]
        Refilling,
        [StatusField(5, 30)]
        Boarding,
        [StatusField(0, 5)]
        Takeoff,
        [StatusField(0, 0)]
        Canceled
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class StatusField : Attribute
    {
        public int Minperiod { get; private set; }
        public int Maxperiod { get; private set; }

        public StatusField(int minPeriod, int maxPeriod)
        {
            Minperiod = minPeriod;
            Maxperiod = maxPeriod;
        }
    }

    public static class EnumExtension
    {
        public static T GetAttribute<T>(this Enum enumval) where T:System.Attribute
        {
            var type = enumval.GetType();
            var memInfo = type.GetMember(enumval.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
