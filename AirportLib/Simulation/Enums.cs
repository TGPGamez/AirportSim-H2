using System;
using System.Text.RegularExpressions;

namespace AirportLib
{
    public enum FlightStatus
    {
        [StatusField(0, 0)]
        [LogMessage("$firstName has booked a ticket to $destination")]
        OpenForReservation,
        [StatusField(360, 900)]
        [LogMessage("$name is now closed for reservations")]
        FarAway,
        [StatusField(70, 360)]
        [LogMessage("$name is 290 min from the airport")]
        OnTheWay,
        [StatusField(60, 70)]
        [LogMessage("$name has just landed")]
        Landing,
        [StatusField(30, 60)]
        [LogMessage("$name is being filled with luggages")]
        Refilling,
        [StatusField(5, 30)]
        [LogMessage("$name is now boading")]
        Boarding,
        [StatusField(0, 5)]
        [LogMessage("$name is about to takeoff with all booked passengers")]
        Takeoff,
        [StatusField(0, 5)]
        [LogMessage("$name is about to takeoff with missing passengers due to bustle")]
        TakeoffMissing,
        [StatusField(0, 0)]
        [LogMessage("$name got cancelled due to insufficient reservations")]
        Canceled
    }

    /// <summary>
    /// Used on FlightStatus as a Attribute and a way to determine 
    /// the right status out from a minimum and maximum time period.
    /// </summary>
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

    /// <summary>
    /// Used on FlightStatus as a Attribute to get the Log Message
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LogMessage : Attribute
    {
        public string Message { get; private set; }
        public LogMessage(string logMessage)
        {
            Message = logMessage;
        }
    }

    /// <summary>
    /// Extensions for enums
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the Custom Attribute class
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="enumval"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum enumval) where T:System.Attribute
        {
            var type = enumval.GetType();
            var memInfo = type.GetMember(enumval.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        /// <summary>
        /// Replaces out from the searchFor with the replaceWith
        /// </summary>
        /// <param name="message"></param>
        /// <param name="searchFor"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        //private static string pattern = @"(\$[^\s]+)";
        //public static string ReplaceWithValue(this string message, string searchFor, string replaceWith)
        //{
        //    string pattern = @"(\$(" + searchFor + "))";
        //    Match match = Regex.Match(message, pattern);

        //    string result = Regex.Replace(message, pattern, replaceWith, RegexOptions.Multiline);
        //    return result;
        //}

        /// <summary>
        /// Replaces out from a Array of searchsFor with each value in the Array repalcesWith
        /// </summary>
        /// <param name="message"></param>
        /// <param name="searchsFor"></param>
        /// <param name="replacesWith"></param>
        /// <returns></returns>
        public static string ReplaceWithValue(this string message, string searchString, string replaceString)
        {
            if (searchString.Contains('|'))
            {
                if (replaceString.Contains('|'))
                {
                    string[] searchsFor = GetValues(searchString);
                    string[] replacesWith = GetValues(replaceString);
                    for (int i = 0; i < searchsFor.Length; i++)
                    {
                        if (replacesWith[i] != null)
                        {
                            message = ReplaceWithValue(message, searchsFor[i], replacesWith[i]);
                        }
                    }
                    return message;
                }
            }
            string pattern = @"(\$(" + searchString + "))";
            Match match = Regex.Match(message, pattern);

            string result = Regex.Replace(message, pattern, replaceString, RegexOptions.Multiline);
            return result;

        }

        private static string[] GetValues(string inputString)
        {
            string[] result = inputString.Split('|');
            return result;
        }
    }
}
