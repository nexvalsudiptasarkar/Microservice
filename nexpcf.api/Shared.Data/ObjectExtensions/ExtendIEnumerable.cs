using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
    public static class ExtendIEnumerable
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        public static T MaxBy<T>(this IEnumerable<T> list, string valuePropertyName)
        {
            T result = default(T);
            PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
            if (info != null && info.PropertyType.IsNumeric())
            {
                using (var looper = list.GetEnumerator())
                {
                    if (!looper.MoveNext())
                    {
                        throw new InvalidOperationException("Sequence contains no elements");
                    }
                    T max = default(T);
                    while (looper.MoveNext())
                    {
                        double maxValue = (double)Convert.ChangeType(info.GetValue(max, null), typeof(double));
                        T item = looper.Current;
                        object propValue = info.GetValue(item, null);
                        double number = (double)Convert.ChangeType(propValue, typeof(double));
                        if (max.Equals(default(T)) || maxValue < number)
                        {
                            max = item;
                        }
                    }
                    result = max;
                }
            }
            return result;
        }

        public static Type GetEnumeratedType<T>(this IEnumerable<T> _)
        {
            return typeof(T);
        }

        #region Trend line calcs for business charts

        public enum TrendType { Exponential, Linear, Logarithmic, PointToPoint, Polynomial, Power, MovingAverage };

        public class PointD
        {
            public double X { get; set; }
            public double Y { get; set; }
            public PointD()
            {
                this.X = Double.NegativeInfinity;
                this.Y = Double.NegativeInfinity;
            }
            public PointD(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public static int IndexOf<T>(this IEnumerable<T> list, T item)
        {
            int index = -1;
            int counter = 0;
            using (var indexer = list.GetEnumerator())
            {
                while (indexer.MoveNext())
                {
                    if (indexer.Current.Equals(item))
                    {
                        index = counter;
                        break;
                    }
                    counter++;
                }
            }
            return index;
        }

        public static IEnumerable<T> SubsetByIndex<T>(this IEnumerable<T> list, int startIndex, int endIndex = -1)
        {
            List<T> result = new List<T>();
            if (list.Count() > 0)
            {
                if (startIndex < 0 || startIndex >= list.Count())
                {
                    if (ExtendList.throwExtensionExceptions)
                    {
                        throw new ArgumentOutOfRangeException((startIndex < 0) ? "startIndex < 0" : "startIndex > list.Count-1");
                    }
                }
                if (endIndex >= 0)
                {
                    if (endIndex < startIndex || endIndex >= list.Count())
                    {
                        throw new ArgumentOutOfRangeException((endIndex < startIndex) ? "endIndex < startIndex" : "endIndex > list.Count-1");
                    }
                }
                else
                {
                    endIndex = list.Count() - 1;
                }

                result = list.Where(x => list.IndexOf(x) >= startIndex && list.IndexOf(x) <= endIndex).ToList();
            }
            return result;
        }

        public static double CalcLeastSquares(IEnumerable<PointD> points, double x)
        {
            double result = 0d;
            if (points != null && points.Count() >= 0)
            {
                double slope = ExtendIEnumerable.CalcSlope(points);
                double yIntercept = ExtendIEnumerable.CalcYIntercept(points, slope);
                result = (slope * x) + yIntercept;
            }
            return result;
        }

        private static double CalcSlope(IEnumerable<PointD> points)
        {
            double result = 0d;
            if (points != null && points.Count() >= 0)
            {
                double xAvg = points.Average(p => p.X);
                double yAvg = points.Average(p => p.Y);
                double dividend = points.Sum(p => (p.X - xAvg) * (p.Y - yAvg));
                double divisor = (double)points.Sum(p => Math.Pow(p.X - xAvg, 2));
                result = dividend / divisor;
            }
            return result;
        }

        private static double CalcYIntercept(IEnumerable<PointD> points, double slope)
        {
            double result = 0d;
            if (points != null && points.Count() >= 0)
            {
                double xAvg = points.Average(p => p.X);
                double yAvg = points.Average(p => p.Y);
                result = yAvg - (slope * xAvg);
            }
            return result;
        }

        public static double[] CalcLinearTrend<T>(this IEnumerable<T> list, PropertyInfo info)
        {
            double[] points = new double[] { 0d, 0d };
            if (list != null && list.Count() > 2 && info != null)
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
                points[0] = ExtendIEnumerable.CalcLeastSquares(data, 1);
                points[1] = ExtendIEnumerable.CalcLeastSquares(data, data.Count);
            }
            return points;
        }

        public static double[] CalcPointToPointTrend<T>(this IEnumerable<T> list, PropertyInfo info)
        {
            double[] points = new double[] { 0d, 0d };
            if (list != null && list.Count() > 2 && info != null)
            {
                int ptIndex = -1;
                for (int i = 0; i < list.Count(); i++)
                {
                    T item = list.ElementAt(i);
                    if (i == 0)
                    {
                        ptIndex = i;
                    }
                    else if (i == list.Count() - 1)
                    {
                        ptIndex = 1;
                    }
                    else
                    {
                        ptIndex = -1;
                    }
                    if (ptIndex != -1)
                    {
                        object propValue = info.GetValue(item, null);
                        double y = (double)Convert.ChangeType(propValue, typeof(double));
                        points[ptIndex] = y;
                    }
                }
            }
            return points;
        }

        public static double[] CalcTrendPoints<T>(this IEnumerable<T> list, PropertyInfo info, TrendType trendType)
        {
            double[] points = new double[] { 0d, 0d };

            switch (trendType)
            {
                #region Linear
                case TrendType.Linear:
                    {
                        points = list.CalcLinearTrend(info);
                    }
                    break;
                #endregion Linear

                #region Point-to-point
                case TrendType.PointToPoint:
                    {
                        points = list.CalcPointToPointTrend(info);
                    }
                    break;
                #endregion Point-to-point

                case TrendType.Exponential:
                case TrendType.Logarithmic:
                case TrendType.MovingAverage:
                case TrendType.Polynomial:
                case TrendType.Power:
                default:
                    throw new Exception(string.Format("TrendType.{0} is not supported at this time.", trendType.ToString()));
            }
            return points;
        }

        public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, TrendType trendType = TrendType.Linear)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count() > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
                    if (info != null && info.PropertyType.IsNumeric())
                    {
                        list.CalcTrendPoints(info, trendType);
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

        public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, string datePropertyName, DateTime startDate, DateTime endDate, TrendType trendType = TrendType.Linear)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count() > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
                    PropertyInfo dateInfo = typeof(T).GetProperty(datePropertyName);

                    if (info != null && info.PropertyType.IsNumeric() &&
                        dateInfo != null && dateInfo.PropertyType.IsDateTime())
                    {
                        var newList = list.Where(x => ((DateTime)(dateInfo.GetValue(x, null))).IsBetween(startDate, endDate, true));
                        newList.CalcTrendPoints(info, trendType);
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

        public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, string datePropertyName, DateTime endDate, int rolling, TrendType trendType = TrendType.Linear)
        {
            DateTime startDate = endDate.AddMonths(-(rolling - 1));
            return list.CalcTrendValues(valuePropertyName, datePropertyName, startDate, endDate, trendType);
        }

        public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, int startIndex, int endIndex = -1, TrendType trendType = TrendType.Linear)
        {
            double[] points = new double[] { 0d, 0d };
            try
            {
                if (list.Count() > 2)
                {
                    PropertyInfo info = typeof(T).GetProperty(valuePropertyName);

                    if (info != null && info.PropertyType.IsNumeric())
                    {
                        var newList = list.SubsetByIndex(startIndex, endIndex);
                        points = newList.CalcTrendPoints(info, trendType);
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

        #endregion Trend line calcs for business charts
    }
}
