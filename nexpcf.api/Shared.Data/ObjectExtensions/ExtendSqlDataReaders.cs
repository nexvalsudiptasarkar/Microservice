using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
    public static class ExtendSqlDataReaders
    {
        public static string GetStringOrDefault(this MySqlDataReader reader, int ordinal, string defaultValue)
        {
            string value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetString(ordinal);
            }
            return value;
        }

        public static Int32 GetInt32OrDefault(this MySqlDataReader reader, int ordinal, Int32 defaultValue)
        {
            Int32 value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetInt32(ordinal);
            }
            return value;
        }

        public static Int64 GetInt64OrDefault(this MySqlDataReader reader, int ordinal, Int64 defaultValue)
        {
            Int64 value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetInt64(ordinal);
            }
            return value;
        }

        public static DateTime GetDateTimeOrDefault(this MySqlDataReader reader, int ordinal, DateTime defaultValue)
        {
            DateTime value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDateTime(ordinal);
            }
            return value;
        }

        public static double GetDoubleOrDefault(this MySqlDataReader reader, int ordinal, double defaultValue)
        {
            double value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDouble(ordinal);
            }
            return value;
        }

        public static decimal GetDecimalOrDefault(this MySqlDataReader reader, int ordinal, decimal defaultValue)
        {
            decimal value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetDecimal(ordinal);
            }
            return value;
        }

        public static bool GetBoolOrDefault(this MySqlDataReader reader, int ordinal, bool defaultValue)
        {
            bool value = defaultValue;
            if (!reader.IsDBNull(ordinal))
            {
                value = reader.GetBoolean(ordinal);
            }
            return value;
        }

        public static void Clear(this DataRow row)
        {
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                DataColumn column = row.Table.Columns[i];
                if (column.DefaultValue != null)
                {
                    if (!column.ReadOnly)
                    {
                        switch (column.DataType.Name.ToLower().Substring(0, 3))
                        {
                            case "str":
                            case "cha":
                                row[i] = "";
                                break;
                            case "int":
                            case "uin":
                            case "sho":
                            case "byt":
                            case "sby":
                            case "dec":
                            case "dou":
                            case "sin":
                                row[i] = 0;
                                break;
                            case "boo":
                                row[i] = false;
                                break;
                            case "dat":
                                row[i] = new DateTime(0);
                                break;
                            case "obj":
                            default:
                                row[i] = DBNull.Value;
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Column {0} is read-only.", i));
                    }
                }
                else
                {
                    row[i] = column.DefaultValue;
                }
            }
        }
    }
}
