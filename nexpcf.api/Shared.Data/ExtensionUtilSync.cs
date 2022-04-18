using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Shared.Data
{
    public static class ExtensionUtilSync
    {
        public static List<T> ToListAsync<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties();
            List<T> result = new List<T>();

            DataTable dt = table;
            foreach (var row in dt.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }
        public static List<T> ToListCustom<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties();
            List<T> result = new List<T>();

            DataTable dt = table;
            foreach (var row in dt.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        public static List<T> ToListAsync<T>(this DataSet dataset) where T : new()
        {
            try {
                IList<PropertyInfo> properties = typeof(T).GetProperties();
                List<T> result = new List<T>();

                DataSet ds = dataset;
                if (ds != null && ds.Tables.Count > default(int))
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        var item = CreateItemFromRow<T>((DataRow)row, properties);
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public static List<T> ToListCustom<T>(this DataSet dataset) where T : new()
        {
            try
            {
                IList<PropertyInfo> properties = typeof(T).GetProperties();
                List<T> result = new List<T>();

                DataSet ds = dataset;
                if (ds != null && ds.Tables.Count > default(int))
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        var item = CreateItemFromRow<T>((DataRow)row, properties);
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                //_logger.LogInformation("GenerateAttendanceSummary failed: {time}", DateTimeOffset.Now);
                throw e;
            }

        }

        //public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> asyncAction, int maxDegreeOfParallelism = 2)
        //{
        //    var semaphoreSlim = new System.Threading.SemaphoreSlim(maxDegreeOfParallelism);
        //    var tcs = new TaskCompletionSource<object>();
        //    var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();
        //    bool addingCompleted = false;

        //    foreach (T item in source)
        //    {
        //        await semaphoreSlim.WaitAsync();
        //        await asyncAction(item).ContinueWith(t =>
        //        {
        //            semaphoreSlim.Release();

        //            if (t.Exception != null)
        //            {
        //                exceptions.Add(t.Exception);
        //            }

        //            if (System.Threading.Volatile.Read(ref addingCompleted) && semaphoreSlim.CurrentCount == maxDegreeOfParallelism)
        //            {
        //                tcs.TrySetResult(null);
        //            }
        //        });
        //    }

        //    System.Threading.Volatile.Write(ref addingCompleted, true);
        //    await tcs.Task;
        //    if (exceptions.Count > 0)
        //    {
        //        throw new AggregateException(exceptions);
        //    }
        //}

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row[property.Name] == DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                        {
                            //nullable
                            object convertedValue = null;
                            try
                            {
                                convertedValue = System.Convert.ChangeType(row[property.Name], Nullable.GetUnderlyingType(property.PropertyType));
                            }
                            catch (Exception ex)
                            {
                            }
                            property.SetValue(item, convertedValue, null);
                        }
                        else
                            property.SetValue(item, row[property.Name], null);
                    }
                }
            }
            return item;
        }
        private static async Task<T> CreateItemFromRowAsync<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row[property.Name] == DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                        {
                            //nullable
                            object convertedValue = null;
                            try
                            {
                                convertedValue = System.Convert.ChangeType(row[property.Name], Nullable.GetUnderlyingType(property.PropertyType));
                            }
                            catch (Exception ex)
                            {
                            }
                            property.SetValue(item, convertedValue, null);
                        }
                        else
                            property.SetValue(item, row[property.Name], null);
                    }
                }
            }
            return item;
        }
    }
}
