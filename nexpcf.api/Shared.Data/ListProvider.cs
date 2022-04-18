﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Xml;

namespace Shared.Data
{
    public class ListProvider<T> where T : new()
    {
        public static List<T> FindAll(IDataReader rdr)
        {
            //instantiate new list of <T> that will be returned
            List<T> returnList = new List<T>();

            //need a Type and PropertyInfo object to set properties via reflection
            Type tType = new T().GetType();
            PropertyInfo pInfo;

            //x will hold the instance of <T> until it is added to the list
            T x;

            //use reader to populate list of objects
            while (rdr.Read())
            {
                x = new T();

                //set property values
                //for this to work, command's column names must match property names in object <T>

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    pInfo = tType.GetProperty(rdr.GetName(i), BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (rdr[i] != DBNull.Value)
                        pInfo.SetValue(x, rdr[i], null);


                }

                //once instance of <T> is populated, add to list
                returnList.Add(x);
            }
            //clean up -- assumes you don't need command anymore
            rdr.Dispose();

            return returnList;
        }

        public static List<T> GetAll(IDataReader rdr)
        {
            List<T> returnList = new List<T>();
            while (rdr.Read())
            {
                returnList.Add(fillObject(rdr));
            }
            rdr.Dispose();
            return returnList;
        }

        public static T Get(IDataReader rdr)
        {
            T objTemp = new T();
            while (rdr.Read())
            {
                objTemp = fillObject(rdr);
            }
            rdr.Dispose();
            return objTemp;
        }

        private static T fillObject(IDataReader rdr)
        {
            Type tType = new T().GetType();
            PropertyInfo pInfo;
            T x = new T();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                pInfo = tType.GetProperty(rdr.GetName(i));
                if (pInfo != null)
                {
                    if (rdr[i] != DBNull.Value)
                        pInfo.SetValue(x, rdr[i], null);
                }
            }
            return x;
        }

        public static MySqlParameter[] GetParameter(object objParameters, bool isIdReq)
        {
            Type myType = objParameters.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            MySqlParameter[] arrSqlParameter = new MySqlParameter[(props.Count)];
            int index = 0;
            foreach (PropertyInfo prop in props)
            {

                object propValue = prop.GetValue(objParameters, null);
                string name = prop.Name;
                if (name.Contains("XmlDoc"))
                {
                    if (propValue != null)
                    {
                        arrSqlParameter[index] = new MySqlParameter("@" + name, SqlDbType.Xml);
                        arrSqlParameter[index].Direction = ParameterDirection.Input;
                        arrSqlParameter[index].Value = ((XmlDocument)propValue).InnerXml;
                    }
                    else
                    {
                        arrSqlParameter[index] = new MySqlParameter("@" + name, SqlDbType.Xml);
                        arrSqlParameter[index].Direction = ParameterDirection.Input;
                        arrSqlParameter[index].Value = null;
                    }

                }

                else if (name != "ID" && (!name.Contains("XmlDoc")))
                {
                    if (prop.GetType() == typeof(DateTime))
                    {
                        DateTime objDate = new DateTime();
                        arrSqlParameter[index] = ((DateTime)propValue != objDate) ? new MySqlParameter("@" + name, propValue) : new MySqlParameter("@" + name, null);
                    }
                    else
                    {
                        arrSqlParameter[index] = new MySqlParameter("@" + name, propValue);
                    }
                }
                index++;
            }

            return arrSqlParameter;
        }
    }
}
