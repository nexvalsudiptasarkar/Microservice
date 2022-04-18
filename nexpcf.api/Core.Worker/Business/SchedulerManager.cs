using MySql.Data.MySqlClient;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Worker.Business
{
    public class SchedulerManager : Disposable
    {
        private MySqlDataHelper sdh = new MySqlDataHelper();
        protected override void cleanup() { }

        /*public List<Zone> GetAllData(Zone obj, string strProc)
        {
            List<Zone> result;
            try
            {
                List<Zone> list = new List<Zone>();
                obj.Opmode = 0;
                MySqlParameter[] parameter = Util.GetParameter(obj);
                object obj2 = this.sdh.ExecuteReader(Util.CS(), strProc, CommandType.StoredProcedure, parameter);
                list = ListProvider<Zone>.FindAll((IDataReader)obj2);
                result = list;
            }
            catch (Exception)
            {
                return new List<Zone>();
            }
            return result;
        }

        public Zone GetDetailsByID(Zone obj, string strProc)
        {
            Zone result;
            try
            {
                List<Zone> list = new List<Zone>();
                obj.Opmode = 1;
                MySqlParameter[] parameter = Util.GetParameter(obj);
                object obj2 = this.sdh.ExecuteReader(Util.CS(), strProc, CommandType.StoredProcedure, parameter);
                list = ListProvider<Zone>.FindAll((IDataReader)obj2);
                result = ((list.Count > 0) ? list[0] : null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int Insert(Zone obj, string strProc)
        {
            try
            {
                obj.Opmode = 2;
                MySqlParameter[] parameter = Util.GetParameter(obj);
                return this.sdh.ExecuteNonQuery(Util.CS(), strProc, CommandType.StoredProcedure, parameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(Zone obj, string strProc)
        {
            try
            {
                obj.Opmode = 4;
                MySqlParameter[] parameter = Util.GetParameter(obj);
                return this.sdh.ExecuteNonQuery(Util.CS(), strProc, CommandType.StoredProcedure, parameter);
            }
            catch (Exception)
            {
                return default(int);
            }
        }*/
    }
}
