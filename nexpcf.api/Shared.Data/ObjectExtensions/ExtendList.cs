using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLXCommon;

namespace ObjectExtensions
{
    public class PointD
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public static class ExtendList
    {
        public static bool throwExtensionExceptions = false;

        public static List<double> Modes(this List<double> list)
        {
            var modesList = list.GroupBy(values => values).Select(valueCluster => new
            {
                Value = valueCluster.Key,
                Occurrence = valueCluster.Count(),
            }).ToList();

            int maxOccurrence = modesList.Max(g => g.Occurrence);

            return modesList.Where(x => x.Occurrence == maxOccurrence && maxOccurrence > 1).Select(x => x.Value).ToList();
        }

        public static double Median(this List<double> list, int roundPlaces = 0)
        {
            List<double> orderedList = list.OrderBy(numbers => numbers).ToList();

            int listSize = orderedList.Count;
            double result;

            if (listSize % 2 == 0) // even
            {
                int midIndex = listSize / 2;
                double v1 = orderedList.ElementAt(midIndex - 1);
                double v2 = orderedList.ElementAt(midIndex);
                result = (v1 + v2) / 2d;
            }
            else // odd
            {
                double element = (double)listSize / 2;
                element = Math.Round(element, MidpointRounding.AwayFromZero);

                result = orderedList.ElementAt((int)(element - 1));
            }

            return Math.Round(result, roundPlaces);
        }

        public static string FindLike(this List<string> list, string text)
        {
            string found = string.Empty;
            foreach (string item in list)
            {
                if (item.IsLike(text))
                {
                    found = item;
                    break;
                }
            }
            return found;
        }

        public static int FindIndexLike(this List<string> list, string text)
        {
            int found = -1;
            for (int index = 0; index < list.Count; index++)
            {
                if (list[index].IsLike(text))
                {
                    found = index;
                    break;
                }
            }
            return found;
        }

        public static string CommaSeparated(this List<string> list)
        {
            StringBuilder result = new StringBuilder();
            foreach (string item in list)
            {
                result.AppendFormat("{0}{1}", (result.Length == 0) ? "" : ",", item);
            }
            return result.ToString();
        }

        public static string ToDelimitedString(this List<string> list, char delimiter)
        {
            StringBuilder result = new StringBuilder();
            foreach (string item in list)
            {
                result.AppendFormat("{0}{1}", (result.Length == 0) ? "" : delimiter.ToString(), item);
            }
            return result.ToString();
        }

        public static string FindExact(this List<string> list, string text)
        {
            string found = string.Empty;
            if (list.Contains(text))
            {
                found = text;
            }
            return found;
        }

        public static IEnumerable<T> FindExact<T>(this IEnumerable<T> list, string valuePropertyName, string text)
        {
            IEnumerable<T> found = Enumerable.Empty<T>();
            try
            {
                PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
                if (info != null && info.CanRead && info.PropertyType == typeof(string) && info.GetIndexParameters().Length == 0)
                {
                    found = (from item in list
                             let value = (string)(info.GetValue(item, null))
                             where value == text
                             select item).ToList();
                }
            }
            catch (Exception)
            {
            }
            return found;
        }

        public static IEnumerable<T> FindLike<T>(this IEnumerable<T> list, string valuePropertyName, string text)
        {
            IEnumerable<T> found = default(IEnumerable<T>);
            try
            {
                PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
                if (info != null && info.CanRead && info.PropertyType == typeof(string) && info.GetIndexParameters().Length == 0)
                {
                    found = (from item in list
                             let value = (string)(info.GetValue(item, null))
                             where value.IsLike(text)
                             select item).ToList();
                }
            }
            catch (Exception)
            {
            }
            return found;
        }

        public static T FindFirstExact<T>(this IEnumerable<T> list, string valuePropertyName, string text)
        {
            T found = default(T);
            try
            {
                found = list.FindExact(valuePropertyName, text).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return found;
        }

        public static T FindLastExact<T>(this IEnumerable<T> list, string valuePropertyName, string text)
        {
            T found = default(T);
            try
            {
                found = list.FindExact(valuePropertyName, text).LastOrDefault();
            }
            catch (Exception)
            {
            }
            return found;
        }

        public static T FindFirstLike<T>(this IEnumerable<T> list, string valuePropertyName, string text)
        {
            T found = default(T);
            try
            {
                found = list.FindLike(valuePropertyName, text).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return found;
        }

        public static T FindLastLike<T>(this IEnumerable<T> list, string valuePropertyName, string text)
        {
            T found = default(T);
            try
            {
                found = list.FindLike(valuePropertyName, text).LastOrDefault();
            }
            catch (Exception)
            {
            }
            return found;
        }

        public static List<T> SubsetByIndex<T>(this List<T> list, int startIndex, int endIndex = -1)
        {
            List<T> result = new List<T>();

            if (startIndex < 0 || startIndex >= list.Count)
            {
                if (ExtendList.throwExtensionExceptions)
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
            }

            if (list.Count > 0)
            {
                endIndex = (endIndex < 0) ? list.Count - 1 : Math.Min(endIndex, list.Count - 1);
                startIndex = Math.Min(endIndex, Math.Max(0, startIndex));
                result = (from item in list
                          where list.IndexOf(item) >= startIndex && list.IndexOf(item) <= endIndex
                          select item).ToList();
            }
            return result;
        }

        public static T FirstNonZeroItem<T>(this List<T> list, string valuePropertyName)
        {
            T result = default(T);
            try
            {
                PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
                var found = (from item in list
                             where (double)Convert.ChangeType(info.GetValue(item, null), typeof(double)) > 0.0d
                             select item).FirstOrDefault();
                result = (T)found;
            }
            catch (Exception ex)
            {
                if (ExtendList.throwExtensionExceptions)
                {
                    throw ex;
                }
            }
            return result;
        }

        public static double HighestValue<T>(this List<T> list, string valuePropertyName, double minValue, int roundUpBy, double multiplyBy = 1d)
        {
            double result = 0d;
            PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
            if (info != null && info.PropertyType.IsNumeric())
            {
                double max = minValue;
                foreach (T item in list)
                {
                    object propValue = info.GetValue(item, null);
                    double value = (double)Convert.ChangeType(propValue, typeof(double));
                    max = Math.Max(max, value * multiplyBy);
                }

                long remainder;
                long quotient = Math.DivRem(Convert.ToInt64(Math.Round(max, 0)), Convert.ToInt64(roundUpBy), out remainder);
                result = (quotient + 1) * roundUpBy;

                if (result - Math.Round(max, 0) <= 1d)
                {
                    result += Math.Round(roundUpBy * 0.5, 0);
                }
            }
            return result;
        }

        #region trend calcs

        public static double LeastSquaresValueAtX(List<PointD> points, double x)
        {
            double slope = ExtendList.SlopeOfPoints(points);
            double yIntercept = ExtendList.YInterceptOfPoints(points, slope);
            return ((slope * x) + yIntercept);
        }

        private static double SlopeOfPoints(List<PointD> points)
        {
            double xBar = points.Average(p => p.X);
            double yBar = points.Average(p => p.Y);
            double dividend = points.Sum(p => (p.X - xBar) * (p.Y - yBar));
            double divisor = (float)points.Sum(p => Math.Pow(p.X - xBar, 2));
            return (dividend / divisor);
        }

        private static double YInterceptOfPoints(List<PointD> points, double slope)
        {
            double xBar = points.Average(p => p.X);
            double yBar = points.Average(p => p.Y);
            return (yBar - (slope * xBar));
        }

        public static double[] CalcTrendBase<T>(this List<T> list, PropertyInfo info)
        {
            double[] points = new double[] { 0d, 0d };
            if (list != null && list.Count > 2 && info != null)
            {
                List<PointD> data = new List<PointD>();
                foreach (T item in list)
                {
                    object propValue = info.GetValue(item, null);
                    double y = (double)Convert.ChangeType(propValue, typeof(double));
                    if (y > 0.0d)
                    {
                        double x = (double)(list.IndexOf(item));
                        data.Add(new PointD(x, y));
                    }
                }
                points[0] = ExtendList.LeastSquaresValueAtX(data, 0);
                points[1] = ExtendList.LeastSquaresValueAtX(data, data.Count - 1);
            }
            return points;
        }

        public static double[] CalcTrendValues<T>(this List<T> list, string propertyName)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(propertyName);
                    if (info != null && info.PropertyType.IsNumeric())
                    {
                        points = list.CalcTrendBase(info);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExtendList.throwExtensionExceptions)
                {
                    throw ex;
                }
            }
            return points;
        }

        public static double[] CalcTrendValues<T>(this List<T> list, string valuePropertyName, string datePropertyName, DateTime startDate, DateTime endDate)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
                    PropertyInfo dateInfo = typeof(T).GetProperty(datePropertyName);

                    if (info != null && info.PropertyType.IsNumeric() &&
                        dateInfo != null && dateInfo.PropertyType.IsDateTime())
                    {
                        var newList = (from item in list
                                       where ((DateTime)(dateInfo.GetValue(item, null))).IsBetween(startDate, endDate, true)
                                       select item).ToList();
                        points = newList.CalcTrendBase(info);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExtendList.throwExtensionExceptions)
                {
                    throw ex;
                }
            }
            return points;
        }

        public static double[] CalcTrendValues<T>(this List<T> list, string propertyName, string datePropName, DateTime endDate, int rolling)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(propertyName);
                    PropertyInfo dateInfo = typeof(T).GetProperty(datePropName);

                    if (info != null && info.PropertyType.IsNumeric() &&
                        dateInfo != null && dateInfo.PropertyType.IsDateTime())
                    {
                        DateTime startDate = endDate.AddMonths(-(rolling - 1));
                        var newList = (from item in list
                                       where ((DateTime)(dateInfo.GetValue(item, null))).IsBetween(startDate, endDate, true)
                                       select item).ToList();
                        points = newList.CalcTrendBase(info);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExtendList.throwExtensionExceptions)
                {
                    throw ex;
                }
            }
            return points;
        }

        public static double[] CalcTrendValuesStartingAtIndex<T>(this List<T> list, string propertyName, int startIndex, int endIndex = -1)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(propertyName);

                    if (info != null && info.PropertyType.IsNumeric())
                    {
                        var newList = list.SubsetByIndex(startIndex, endIndex);
                        points = newList.CalcTrendBase(info);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExtendList.throwExtensionExceptions)
                {
                    throw ex;
                }
            }
            return points;
        }

        #endregion trend calcs
    }
}


