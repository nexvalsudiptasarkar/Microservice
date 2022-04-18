using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
    public static class ExtendDateTimes
    {
        public enum ScheduleMode
        {
            Hourly,
            Daily,
            Weekly,
            FirstDayOfMonth,
            NthWeekday,
            LastDayOfMonth,
            DayOfMonth,
            SpecificInterval
        };

        #region Date comparison

        public enum DateCompareState
        {
            Equal = 0,
            Earlier = 1,
            Later = 2
        };

        [Flags]
        public enum DatePartFlags
        {
            Ticks = 0,
            Year = 1,
            Month = 2,
            Day = 4,
            Hour = 8,
            Minute = 16,
            Second = 32,
            Millisecond = 64
        };

        public static bool PartiallyEqual(this DateTime then, DateTime now, DatePartFlags flags = DatePartFlags.Ticks)
        {
            bool isEqual = false;
            if (flags == DatePartFlags.Ticks)
            {
                isEqual = (now == then);
            }
            else
            {
                StringBuilder compareStr = new StringBuilder();
                compareStr.Append(flags.HasFlag(DatePartFlags.Year) ? "yyyy" : "");
                compareStr.Append(flags.HasFlag(DatePartFlags.Month) ? "MM" : "");
                compareStr.Append(flags.HasFlag(DatePartFlags.Day) ? "dd" : "");
                compareStr.Append(flags.HasFlag(DatePartFlags.Hour) ? "HH" : "");
                compareStr.Append(flags.HasFlag(DatePartFlags.Minute) ? "mm" : "");
                compareStr.Append(flags.HasFlag(DatePartFlags.Second) ? "ss" : "");
                compareStr.Append(flags.HasFlag(DatePartFlags.Millisecond) ? "fff" : "");
                isEqual = now.ConvertToInt64(compareStr.ToString()) == then.ConvertToInt64(compareStr.ToString());
            }
            return isEqual;
        }

        public static DateCompareState CompareDatesExtern(DateTime thisDate, DateTime thatDate, bool includeTime = false, DatePartFlags timeFlags = DatePartFlags.Hour | DatePartFlags.Minute)
        {
            return thisDate.CompareDates(thatDate, includeTime, timeFlags);
        }

        public static DateCompareState CompareDates(this DateTime thisDate, DateTime thatDate, bool includeTime = false, DatePartFlags timeFlags = DatePartFlags.Hour | DatePartFlags.Minute)
        {
            DateCompareState state = DateCompareState.Equal;

            TimeSpan thisTime = (includeTime) ? new TimeSpan(timeFlags.HasFlag(DatePartFlags.Hour) ? thisDate.Hour : 0,
                                                                timeFlags.HasFlag(DatePartFlags.Minute) ? thisDate.Minute : 0,
                                                                timeFlags.HasFlag(DatePartFlags.Second) ? thisDate.Second : 0,
                                                                timeFlags.HasFlag(DatePartFlags.Millisecond) ? thatDate.Millisecond : 0)
                                                : new TimeSpan(0);
            TimeSpan thatTime = (includeTime) ? new TimeSpan(timeFlags.HasFlag(DatePartFlags.Hour) ? thatDate.Hour : 0,
                                                                timeFlags.HasFlag(DatePartFlags.Minute) ? thatDate.Minute : 0,
                                                                timeFlags.HasFlag(DatePartFlags.Second) ? thatDate.Second : 0,
                                                                timeFlags.HasFlag(DatePartFlags.Millisecond) ? thatDate.Millisecond : 0)
                                                : new TimeSpan(0);

            thisDate = thisDate.SetTime(thisTime);
            thatDate = thatDate.SetTime(thatTime);

            long thisValue = thisDate.Ticks;
            long thatValue = thatDate.Ticks;

            if (thisValue < thatValue)
            {
                state = DateCompareState.Earlier;
            }
            else
            {
                if (thisValue > thatValue)
                {
                    state = DateCompareState.Later;
                }
            }
            return state;
        }

        public static DateTime Highest(this DateTime date, DateTime thatDate)
        {
            //DateTime result = (date < thatDate) ? thatDate : date;
            DateTime result = new DateTime(Math.Max(date.Ticks, thatDate.Ticks));
            return result;
        }

        public static DateTime Lowest(this DateTime date, DateTime thatDate)
        {
            //DateTime result = (date > thatDate) ? thatDate : date;
            DateTime result = new DateTime(Math.Min(date.Ticks, thatDate.Ticks));
            return result;
        }

        public static int DateDiff(this DateTime date, DateTime thatDate, DatePartFlags datePart)
        {
            int result = 0;
            switch (datePart)
            {
                case DatePartFlags.Day:
                    result = (int)((date.Date - thatDate.Date).TotalDays);
                    break;
                case DatePartFlags.Month:
                    result = Math.Abs(((date.Year * 12) + date.Month) - ((thatDate.Year * 12) + thatDate.Month));
                    break;
                case DatePartFlags.Year:
                    result = Math.Abs(date.Year - thatDate.Year);
                    break;
            }
            return result;
        }

        public static bool IsBetween(this DateTime date, DateTime start, DateTime end, bool inclusive)
        {
            bool result = false;
            DateTime d1 = start.Lowest(end);
            DateTime d2 = start.Highest(end);
            if (inclusive)
            {
                result = (date >= d1 && date <= d2);
            }
            else
            {
                result = (date == d1 && date == d2);
            }
            return result;
        }

        #endregion

        #region Fiscal dates

        private static int fiscalYearStartMonth = 10;

        public static int FiscalYearStartMonth
        {
            get { return ExtendDateTimes.fiscalYearStartMonth; }
            set { ExtendDateTimes.fiscalYearStartMonth = value; }
        }

        public static DateTime ToFiscalDate(this DateTime thisDate, int fiscalStartMonth)
        {
            ExtendDateTimes.fiscalYearStartMonth = fiscalStartMonth;
            return thisDate.ToFiscalDate();
        }

        public static DateTime FromFiscalDate(this DateTime thisDate, int fiscalStartMonth)
        {
            ExtendDateTimes.fiscalYearStartMonth = fiscalStartMonth;
            return thisDate.FromFiscalDate();
        }

        public static DateTime ToFiscalDate(this DateTime thisDate)
        {
            DateTime newDate = new DateTime(0);
            try
            {
                newDate = thisDate.AddMonths(13 - ExtendDateTimes.fiscalYearStartMonth);
            }
            catch (Exception)
            {
            }
            return newDate;
        }

        public static DateTime FromFiscalDate(this DateTime thisDate)
        {
            DateTime newDate = new DateTime(0);
            try
            {
                newDate = thisDate.AddMonths(-(13 - ExtendDateTimes.fiscalYearStartMonth));
            }
            catch (Exception)
            {
            }
            return newDate;
        }

        #endregion Fiscal dates

        #region Set date components

        public static int LastDayOfMonth(this DateTime thisDate)
        {
            int lastDay = thisDate.AddMonths(1).AddDays(-1).Day;
            return lastDay;
        }

        public static DateTime SetDay(this DateTime thisDate, int year, int month, int day, bool zeroTime = false)
        {
            if (year < 0 || year > DateTime.MaxValue.Year)
            {
                throw new InvalidOperationException("Year is invalid.");
            }
            if (month < 1 || month > 12)
            {
                throw new InvalidOperationException("Month must be from 1 to 12.");
            }
            int lastDay = new DateTime(year, month, 1).LastDayOfMonth();
            if (day < 1 || day > lastDay)
            {
                throw new InvalidOperationException(string.Format("Day must be from 1 to {0}.", lastDay));
            }

            DateTime newDate = thisDate;
            newDate = newDate.SetYear(year);
            newDate = newDate.SetMonth(month);
            newDate = newDate.SetDay(day);
            if (zeroTime)
            {
                newDate = newDate.Date;
            }
            return newDate;
        }

        public static DateTime SetDay(this DateTime thisDate, int day)
        {
            int lastDay = thisDate.DaysInMonth();
            if (day < 1 || day > lastDay)
            {
                throw new ArgumentException(string.Format("The specified day ({0}) is an invalid value.", day));
            }
            return (new DateTime(thisDate.Year, thisDate.Month, day, thisDate.Hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
        }

        public static DateTime SetMonth(this DateTime thisDate, int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentException(string.Format("The specified month ({0}) is an invalid value.", month));
            }
            return (new DateTime(thisDate.Year, month, thisDate.Day, thisDate.Hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
        }

        public static DateTime SetYear(this DateTime thisDate, int year)
        {
            if (year < 1 || year > DateTime.MaxValue.Year)
            {
                throw new ArgumentException(string.Format("The specified year ({0}) is an invalid value.", year));
            }
            return (new DateTime(year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
        }

        public static DateTime SetHour(this DateTime thisDate, int hour)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentException(string.Format("The specified hour ({0}) is an invalid value.", hour));
            }
            return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
        }

        public static DateTime SetMinute(this DateTime thisDate, int minute)
        {
            if (minute < 0 || minute > 59)
            {
                throw new ArgumentException(string.Format("The specified minute ({0}) is an invalid value.", minute));
            }
            return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
        }

        public static DateTime SetSecond(this DateTime thisDate, int second)
        {
            if (second < 0 || second > 59)
            {
                throw new ArgumentException(string.Format("The specified minute ({0}) is an invalid value.", second));
            }
            return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, second, thisDate.Millisecond, thisDate.Kind));
        }

        public static DateTime SetMillisecond(this DateTime thisDate, int millisecond)
        {
            if (millisecond < 0 || millisecond > 999)
            {
                throw new ArgumentException(string.Format("The specified millisecond ({0}) is an invalid value.", millisecond));
            }
            return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, thisDate.Second, millisecond, thisDate.Kind));
        }

        public static DateTime ZeroSeconds(this DateTime thisDate)
        {
            return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, 0, 0, thisDate.Kind));
        }

        public static DateTime ZeroTime(this DateTime thisDate)
        {
            //return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, 0, 0, 0, 0, thisDate.Kind));
            return thisDate.Date;
        }

        public static DateTime SetTime(this DateTime thisDate, TimeSpan span)
        {
            if (span == null)
            {
                throw new ArgumentNullException("Span is null");
            }
            return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, span.Hours, span.Minutes, span.Seconds, span.Milliseconds, thisDate.Kind));
        }

        public static DateTime SetTime(this DateTime thisDate, int hours, int mins, int secs = 0, int millisecs = 0)
        {
            if (hours > 23)
            {
                throw new InvalidOperationException("Hours must be < 24.");
            }
            if (mins > 59)
            {
                throw new InvalidOperationException("Minutes must be < 60.");
            }
            if (secs > 59)
            {
                throw new InvalidOperationException("Seconds must be < 60.");
            }
            if (millisecs > 999)
            {
                throw new InvalidOperationException("Milliseconds must be < 1000.");
            }
            DateTime newDate = thisDate;
            newDate = (hours < 0) ? newDate : newDate.SetHour(hours);
            newDate = (mins < 0) ? newDate : newDate.SetMinute(mins);
            newDate = (secs < 0) ? newDate : newDate.SetSecond(secs);
            newDate = (millisecs < 0) ? newDate : newDate.SetMillisecond(millisecs);
            return newDate;
        }

        public static DateTime Normalize(this DateTime date)
        {
            return (date.SetDay(1).Date);
        }

        public static DateTime GetNextDayOfWeek(this DateTime thisDate, DayOfWeek dayOfWeek)
        {
            DateTime newDate = thisDate;
            do
            {
                newDate = newDate.AddDays(1);
            } while (newDate.DayOfWeek != dayOfWeek);
            return newDate;
        }

        #endregion Set date components

        #region Helper methods

        public static int DaysInMonth(this DateTime thisDate)
        {
            return CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(thisDate.Year, thisDate.Month);
        }

        public static bool IsLastDayOfMonth(this DateTime thisDate)
        {
            return (thisDate.Day == thisDate.DaysInMonth());
        }

        public static bool IsWeekend(this DateTime thisDate)
        {
            return (thisDate.DayOfWeek == DayOfWeek.Saturday || thisDate.DayOfWeek == DayOfWeek.Sunday);
        }

        public static bool IsLeapYear(this DateTime thisDate)
        {
            return CultureInfo.InvariantCulture.Calendar.IsLeapYear(thisDate.Year);
        }

        public static DateTime GetFutureDateTime(this DateTime thisDate, TimeSpan processTime, ScheduleMode mode, DayOfWeek dayOfWeek = DayOfWeek.Sunday, int ordinal = -1)
        {
            DateTime result = thisDate;

            if (processTime == null)
            {
                throw new Exception("Parameter 2 (processTime) cannot be null");
            }

            switch (mode)
            {
                case ScheduleMode.Hourly:
                    {
                        if (result.Minute < processTime.Minutes)
                        {
                            result = result.AddMinutes(processTime.Minutes - result.Minute);
                        }
                        else
                        {
                            result = result.Subtract(new TimeSpan(0, 0, result.Minute, result.Second, result.Millisecond));
                            result = result.Add(new TimeSpan(0, 1, processTime.Minutes, 0, 0));
                        }
                    }
                    break;
                case ScheduleMode.Daily:
                    {
                        result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
                        result = result.Add(new TimeSpan(1, processTime.Hours, processTime.Minutes, 0, 0));
                    }
                    break;
                case ScheduleMode.Weekly:
                    {
                        int daysToAdd = 7;
                        if (result.DayOfWeek != dayOfWeek)
                        {
                            int dayNumber = (int)dayOfWeek;
                            daysToAdd = ((int)(result.DayOfWeek) < dayNumber) ? (int)(result.DayOfWeek) - dayNumber : 7 - dayNumber;
                        }
                        result = result.AddDays(daysToAdd);
                        result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
                        result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
                    }
                    break;
                case ScheduleMode.NthWeekday:
                    {
                        ordinal = Math.Max(ordinal, 0);
                        result = result.GetDateByOrdinalDay(dayOfWeek, ordinal);
                        if (result < thisDate)
                        {
                            result = result.AddMonths(1).GetDateByOrdinalDay(dayOfWeek, ordinal);
                        }
                        result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
                        result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
                    }
                    break;
                case ScheduleMode.FirstDayOfMonth:
                    {
                        int daysThisMonth = DaysInMonth(result);
                        int today = result.Day;
                        if (today == 1)
                        {
                            result = result.AddDays(daysThisMonth);
                        }
                        else
                        {
                            result = result.AddDays((daysThisMonth - today) + 1);
                        }
                        result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
                        result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
                    }
                    break;
                case ScheduleMode.LastDayOfMonth:
                    {
                        int daysThisMonth = DaysInMonth(result);
                        int today = result.Day;
                        if (today == daysThisMonth)
                        {
                            int daysNextMonth = DaysInMonth(result.AddDays(1));
                            result = result.AddDays(daysNextMonth);
                        }
                        else
                        {
                            result = result.AddDays(daysThisMonth - today);
                        }
                        result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
                        result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
                    }
                    break;
                case ScheduleMode.DayOfMonth:
                    {
                        int leapDay = 0;
                        if (result.Month == 2 && !IsLeapYear(result) && processTime.Days == 29)
                        {
                            leapDay = 1;
                        }

                        int daysToAdd = 0;
                        if (result.Day < processTime.Days)
                        {
                            daysToAdd = processTime.Days - result.Day - leapDay;
                        }
                        else
                        {
                            daysToAdd = (DaysInMonth(result) - result.Day) + processTime.Days - leapDay;
                        }
                        result = result.AddDays(daysToAdd);
                        result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
                        result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
                    }
                    break;
                case ScheduleMode.SpecificInterval:
                    {
                        if (result.Second >= 30)
                        {
                            result = result.AddSeconds(60 - result.Second);
                        }

                        result = result.Subtract(new TimeSpan(0, 0, 0, result.Second, result.Millisecond));
                        result = result.Add(processTime);
                    }
                    break;
            }

            result = result.Subtract(new TimeSpan(0, 0, 0, result.Second, result.Millisecond));
            return result;
        }

        public static long ConvertToInt64(this DateTime thisDate)
        {
            long dateValue = Convert.ToInt64(thisDate.ToString("yyyyMMddHHmm"));
            return dateValue;
        }

        public static long ConvertToInt64(this DateTime thisDate, string format)
        {
            long dateValue = Convert.ToInt64(thisDate.ToString(format));
            return dateValue;
        }

        private static int ThisCentury(int year)
        {
            int result = (int)((double)year * 0.01);
            return result;
        }
        private static int ThisCentury(this DateTime thisDate)
        {
            int result = (int)((double)thisDate.Year * 0.01);
            return result;
        }

        public static DateTime FromMonthYearString(this DateTime date, string data)
        {
            DateTime newDate = new DateTime(0);
            string[] parts = data.Split('-');
            if (parts.Length == 2)
            {
                if (parts[0].Length == 3 && (parts[1].Length == 2 || parts[1].Length == 4))
                {
                    if (parts[1].Length == 2)
                    {
                        int value = Convert.ToInt32(parts[1]);
                        int year = (DateTime.Now.Year - value) + value;
                        parts[1] = string.Format("{0}", year);
                    }
                    data = string.Format("{0}-01-{2}", parts[0], parts[1]);
                    try
                    {
                        newDate = DateTime.ParseExact(data, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                    catch (Exception)
                    {
                        newDate = new DateTime(0);
                    }
                }
            }
            return newDate;
        }

        public static DateTime FromDayMonthYearString(this DateTime date, string data)
        {
            DateTime newDate = new DateTime(0);
            string[] parts = data.Split('-');
            if (parts.Length == 3)
            {
                if (parts[1].Length == 3 && (parts[2].Length == 2 || parts[2].Length == 4))
                {
                    if (parts[2].Length == 2)
                    {
                        int value = Convert.ToInt32(parts[2]);
                        int year = (DateTime.Now.Year - value) + value;
                        parts[2] = string.Format("{0}", year);
                    }
                    data = string.Format("{0}-{1:00}-{2}", parts[1], Convert.ToInt32(parts[0]), Convert.ToInt32(parts[2]));
                    try
                    {
                        newDate = DateTime.ParseExact(data, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                    catch (Exception)
                    {
                        newDate = new DateTime(0);
                    }
                }
            }
            return newDate;
        }

        public static DateTime FromMonthYear(int month, int year)
        {
            DateTime result = new DateTime(year, month, 1);
            return result;
        }

        public static string ToAccessDateTime(this DateTime date, bool includeTime = false)
        {
            string format = "#{0}{1}#";
            string dateStr = date.ToString("MM/dd/yyyy");
            string timeStr = (includeTime) ? date.ToString(" hh:MM:ss") : "";
            string result = string.Format(format, dateStr, timeStr);
            return result;
        }


        public static DateTime GetDateByOrdinalDay(this DateTime dt, DayOfWeek dayOfWeek, int ordinal)
        {
            DateTime newDate;
            if (ordinal == 0)
            {
                newDate = dt.LastDayOfWeek(dayOfWeek);
            }
            else
            {
                ordinal = Math.Min(Math.Max(ordinal, 1), 5);
                newDate = new DateTime(dt.Year, dt.Month, 1).SetTime(dt.TimeOfDay);
                int diff = 7 - Math.Abs((int)dayOfWeek - (int)newDate.DayOfWeek);
                newDate = (diff == 7) ? newDate : newDate.AddDays(diff);
                if (ordinal > 1)
                {
                    newDate = newDate.AddDays(7 * (ordinal - 1));
                }
            }
            return newDate;
        }

        public static int CountWeekDays(this DateTime thisDate, DateTime thatDate)
        {
            int days = Math.Abs((thisDate - thatDate).Days) + 1;
            return ((days / 7) * 5) + (days % 7);
        }

        public static int CountDayOfWeek(this DateTime thisDate, DayOfWeek dow)
        {
            int count = 4;
            DateTime newDate = thisDate.GetDateByOrdinalDay(dow, 1);
            count = (newDate.DaysInMonth() - newDate.Day) % 7;
            return count;
        }

        public static DateTime LastDayOfWeek(this DateTime thisDate, DayOfWeek dow)
        {
            DateTime newDate = thisDate.SetDay(1).AddMonths(1);
            while (!(newDate.DayOfWeek == dow && newDate.Month == thisDate.Month))
            {
                newDate = newDate.AddDays(-1);
            }
            return newDate;
        }

        public static int CountWeekday(this DateTime thisDate, DateTime thatDate, DayOfWeek dayOfWeek)
        {
            int result = 0;
            DateTime past = (thisDate > thatDate) ? thatDate : thisDate;
            DateTime future = (thisDate > thatDate) ? thisDate : thatDate;
            int totalDays = (future - past).Days + 1;
            int temp = (int)((TimeSpan.FromTicks(Math.Abs(future.Ticks - past.Ticks))).TotalDays);

            int remainder = totalDays % 7;
            int adj = future.DayOfWeek - dayOfWeek;

            result = totalDays / 7;
            adj += (adj < 0) ? 7 : 0;
            result += (remainder >= adj) ? 1 : 0;
            return result;
        }

        public static bool IsBetween(this DateTime thisDate, DateTime date1, DateTime date2)
        {
            bool result = false;
            if (date1 != date2)
            {
                result = (thisDate.Ticks >= Math.Min(date1.Ticks, date2.Ticks) &&
                          thisDate.Ticks <= Math.Max(date1.Ticks, date2.Ticks));
            }
            return result;
        }

        public static bool IsBetween(this TimeSpan thisTime, TimeSpan time1, TimeSpan time2)
        {
            bool result = false;
            if (time1.TotalMilliseconds != time2.TotalMilliseconds)
            {
                result = (thisTime.TotalMilliseconds >= Math.Min(time1.TotalMilliseconds, time2.TotalMilliseconds) &&
                          thisTime.TotalMilliseconds <= Math.Max(time1.TotalMilliseconds, time2.TotalMilliseconds));
            }
            return result;
        }

        public static DateTime FirstDayOfWeek(this DateTime thisDate)
        {
            DateTime newDate = thisDate;
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            newDate = newDate.AddDays(-(newDate.DayOfWeek - cultureInfo.DateTimeFormat.FirstDayOfWeek));
            return newDate;
        }

        public static DateTime NormalizeToMinute(this DateTime thisDate, int delta = 45)
        {
            DateTime newDate = thisDate;
            int minutesToAdd = (newDate.Second < delta) ? 1 : 2;
            newDate = newDate.AddMinutes(minutesToAdd).SetSecond(0).SetMillisecond(0);
            return newDate;
        }

        #endregion Helper methods
    }
}
