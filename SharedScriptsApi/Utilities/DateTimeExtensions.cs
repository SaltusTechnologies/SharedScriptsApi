using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SharedScriptsApi.Utilities
{
    public static class DateTimeExtensions
    {
        #region const

        private const CalendarWeekRule CalendarWeekRule = System.Globalization.CalendarWeekRule.FirstDay;

        #endregion

        #region static fields/properties

        private static readonly Calendar Calendar = CultureInfo.CurrentCulture.Calendar;

        #endregion

        #region static methods

        public static DateTime ConvertToTimezone(this DateTime source, string timeZone) 
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(source, timeZone);
        }

        public static bool IsEqualToYears(this DateTime source, DateTime value)
        {
            return source.ToUniversalTime().RoundToYear().Equals(value.ToUniversalTime().RoundToYear());
        }

        public static bool IsEqualToMonths(this DateTime source, DateTime value)
        {
            return source.ToUniversalTime().RoundToMonth().Equals(value.ToUniversalTime().RoundToMonth());
        }

        public static bool IsEqualToDays(this DateTime source, DateTime value)
        {
            return source.ToUniversalTime().RoundToDay().Equals(value.ToUniversalTime().RoundToDay());
        }

        public static bool IsEqualToHours(this DateTime source, DateTime value)
        {
            return source.ToUniversalTime().RoundToHour().Equals(value.ToUniversalTime().RoundToHour());
        }

        public static bool IsEqualToMinutes(this DateTime source, DateTime value)
        {
            return source.ToUniversalTime().RoundToMinute().Equals(value.ToUniversalTime().RoundToMinute());
        }

        public static bool IsEqualToSeconds(this DateTime source, DateTime value)
        {
            return source.ToUniversalTime().RoundToSecond().Equals(value.ToUniversalTime().RoundToSecond());
        }

        public static DateTime RoundToYear(this DateTime source)
        {
            return new DateTime(source.Year, 1, 1, 0, 0, 0, 0, source.Kind);
        }

        public static DateTime RoundToMonth(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, 1, 0, 0, 0, 0, source.Kind);
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ToDay")]
        public static DateTime RoundToDay(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0, 0, source.Kind);
        }

        public static DateTime RoundToHour(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, 0, 0, 0, source.Kind);
        }

        public static DateTime RoundToMinute(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, 0, 0, source.Kind);
        }

        public static DateTime RoundToSecond(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, 0, source.Kind);
        }

        public static DateTime GetLastDayOfWeek(this DateTime source)
        {
            return RoundToDay(source.AddDays(6 - (int)source.DayOfWeek));
        }

        public static DateTime GetFirstDayOfWeek(this DateTime source)
        {
            return RoundToDay(source.AddDays(-(int)source.DayOfWeek));
        }

        public static bool IsWeekday(this DateTime source)
        {
            return !IsWeekend(source);
        }

        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Sunday || source.DayOfWeek == DayOfWeek.Saturday;
        }

        public static DateTime AddWorkdays(this DateTime source, int value)
        {
            DateTime retVal = source;
            int interval = value > 0 ? 1 : -1;
            while (value != 0)
            {
                retVal = retVal.AddDays(interval);
                if (IsWeekday(retVal))
                {
                    value -= interval;
                }
            }
            return retVal;
        }

        public static int GetDaysInMonth(this DateTime source)
        {
            return DateTime.DaysInMonth(source.Year, source.Month);
        }

        public static int GetWeekOfYear(this DateTime source, DayOfWeek dayOfWeek)
        {
            return Calendar.GetWeekOfYear(source, CalendarWeekRule, dayOfWeek);
        }

        public static int GetWeekOfYear(this DateTime source)
        {
            return GetWeekOfYear(source, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }

        public static DateTime GetFirstDayOfMonth(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, DateTime.DaysInMonth(source.Year, source.Month));
        }

        public static int GetWeekOfMonth(this DateTime source)
        {
            return GetWeekOfYear(source) - GetWeekOfYear(GetFirstDayOfMonth(source)) + 1;
        }

        public static int GetWeeksInMonth(this DateTime source)
        {
            return GetWeekOfYear(GetLastDayOfMonth(source)) - GetWeekOfYear(GetFirstDayOfMonth(source)) + 1;
        }

        public static bool IsValidSqlDate(this DateTime source)
        {
            return source >= new DateTime(1753, 1, 1);
        }

        #endregion
    }
}
