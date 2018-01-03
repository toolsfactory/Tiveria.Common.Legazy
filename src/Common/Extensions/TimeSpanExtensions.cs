using System;

namespace Tiveria.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        /* Buggy
        /// <summary>Als string mit Format: "HH:mm:ss.fff"</summary>
        public static string ToStringEx(this TimeSpan time, bool showDays, bool showMilli)
        {
            string result;
            string milliformat = (showMilli) ? ".{4:00}" : "";
            string daysformat = (time.Days > 0) ? "{0:00}:" : "";
            int len = (showMilli) ? 11 : 8;
            len += (showMilli && (daysformat != "")) ? 3 : 0;
            if (showDays)
                result = string.Format(daysformat + "{1:00}:{2:00}:{3:00}" + milliformat, time.Days, time.Hours, time.Minutes, time.Seconds, time.Milliseconds).Substring(0, len);
            else
                result = string.Format("{0:00}:{1:00}:{2:00}" + milliformat, time.TotalHours, time.Minutes, time.Seconds, null, time.Milliseconds).Substring(0, len);
            return result;
        }

        public static string ToStringEx(this TimeSpan time, string format)
        {
            return string.Format(format, time.Days, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }
         */ 
    }
}
