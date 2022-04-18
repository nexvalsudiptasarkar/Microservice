using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Data
{
    public class MySqlDataHelperSync
    {
        #region [Variable Declaration]
        private int DataCmdTimeout = 120;
        private MySqlCommand cmd = new MySqlCommand();
        public MySqlConnection DataConn = new MySqlConnection();
        #endregion [Variable Declaration]

        #region [Constructor]
        public MySqlDataHelperSync() { }
        #endregion [Constructor]

        #region [Public Methods]
        public int ExecuteNonQuery(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            try
            {                /*_logger.LogInformation(string.Format("{0} :: {1} :: Start at {2}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now
                ));*/
                int i = ExecuteNonQueryThreadSafe(strConn, strProc, cmdType, arrPrm,_logger);
                return i;
            }
            catch (Exception err)
            {
                /*_logger.LogCritical(string.Format("{0} :: {1} :: Exception occured at {2} ERR:- {3}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now,
                    err.Message
                ));*/
                return 0;
            }
        }

        public int ExecuteNonQueryThreadSafe(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            int result = default(int);
            /*_logger.LogInformation(string.Format("{0} :: {1} :: Start at {2}",
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                DateTime.Now
            ));*/
            try
            {
                /*_logger.LogInformation(string.Format("{0} :: {1} :: Start at {2}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now
                ));*/
                using (MySqlConnection dataConnection = new MySqlConnection(strConn))
                {
                    /*_logger.LogInformation(string.Format("{0} :: {1} :: data connection Start at {2}",
                        this.GetType().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now
                    ));*/
                    using (MySqlCommand icmd = dataConnection.CreateCommand())
                    {
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: Create Command Start at {2}",
                            this.GetType().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            DateTime.Now
                        ));*/
                        dataConnection.ConnectionString = strConn;
                        dataConnection.Open();
                        icmd.Connection = dataConnection;
                        icmd.CommandTimeout = DataCmdTimeout;
                        icmd.CommandText = strProc;
                        icmd.CommandType = cmdType;
                        int i = arrPrm.GetUpperBound(0);
                        int j = 0;
                        icmd.Parameters.Clear();
                        foreach (MySqlParameter p in arrPrm)
                        {
                            if (j <= i)
                            {
                                icmd.Parameters.Add(p);
                            }
                            j++;
                        }
                        result =  icmd.ExecuteNonQuery();

                        if (dataConnection.State == ConnectionState.Open)
                        {
                            dataConnection.Close();
                        }
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: Create Command end at {2}",
                            this.GetType().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            DateTime.Now
                        ));*/
                    }
                    /*_logger.LogInformation(string.Format("{0} :: {1} :: end at {2}",
                        this.GetType().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now
                    ));*/
                }
                /*_logger.LogInformation(string.Format("{0} :: {1} :: End at {2}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now
                ));*/
                return result;
            }
            catch (Exception err) {
                _logger.LogCritical(string.Format("{0} :: {1} :: Exception occured at {2} ERR:- {3}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now,
                    err.Message
                ));
                return 0;
            }

        }

        public object ExecuteScalar(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            Tuple<MySqlCommand, MySqlConnection> tplObj = PrepareCommand(strConn, strProc, cmdType, arrPrm, _logger);
            var obj = tplObj.Item1.ExecuteScalar();
            if (tplObj.Item2.State == ConnectionState.Open)
            {
                tplObj.Item2.Close();
                tplObj.Item2.Dispose();
            }
            return obj;
        }

        public async Task<string> ExecuteNonQueryReturnOutput(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            Tuple<MySqlCommand, MySqlConnection> tplObj = PrepareCommand(strConn, strProc, cmdType, arrPrm, _logger);
            string outputVal = arrPrm.SingleOrDefault(x => x.Direction == ParameterDirection.Output).ParameterName;
            var obj = await tplObj.Item1.ExecuteNonQueryAsync();
            if (tplObj.Item2.State == ConnectionState.Open)
            {
                tplObj.Item2.Close();
                tplObj.Item2.Dispose();
            }
            return Convert.ToString(tplObj.Item1.Parameters[outputVal].Value);
        }

        public object ExecuteReader(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            try
            {
                Tuple<MySqlCommand, MySqlConnection> tplObj = PrepareCommand(strConn, strProc, cmdType, arrPrm, _logger);
                MySqlDataReader reader = tplObj.Item1.ExecuteReader();
                if (tplObj.Item2.State == ConnectionState.Open)
                {
                    tplObj.Item2.Close();
                    tplObj.Item2.Dispose();
                }
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ExecuteDataSet(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            try
            {
                /*_logger.LogInformation(string.Format("{0} :: {1} :: Start at {2}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now
                ));*/
                Tuple<MySqlCommand, MySqlConnection> tplObj = PrepareCommand(strConn, strProc, cmdType, arrPrm, _logger);
                MySqlDataAdapter da = new MySqlDataAdapter(tplObj.Item1);
                DataSet ds = new DataSet();
                da.FillAsync(ds);
                /*_logger.LogInformation(string.Format("{0} :: {1} :: data filling completed at {2}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now
                ));*/
                if (tplObj.Item2.State == ConnectionState.Open)
                {
                    tplObj.Item2.Close();
                    tplObj.Item2.Dispose();
                }
                return ds;
            }
            catch (Exception err)
            {
                _logger.LogCritical(string.Format("{0} :: {1} :: Exception occured at {2} ERR:- {3}",
                    this.GetType().Name,
                    "ExecuteDataSet",
                    DateTime.Now,
                    err.Message.ToString()
                ));
                return null;
            }
        }
        #endregion [Public Methods]

        #region [Private Methods]        
        private Tuple<MySqlCommand, MySqlConnection> PrepareCommand(string strConn, string strProc, CommandType cmdType, MySqlParameter[] arrPrm, ILogger _logger)
        {
            try
            {
                /*_logger.LogInformation(string.Format("{0} :: {1} :: Start at {2}",
                   this.GetType().Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name,
                   DateTime.Now
                ));*/
                MySqlCommand icmd;
                MySqlConnection dataConnection;
                Tuple<MySqlCommand, MySqlConnection> tplObj;
                using (dataConnection = new MySqlConnection(strConn))
                {
                    /*_logger.LogInformation(string.Format("{0} :: {1} :: data connection Start at {2}",
                        this.GetType().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now
                    ));*/
                    using (icmd = dataConnection.CreateCommand())
                    {
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: create command Start at {2}",
                           this.GetType().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           DateTime.Now
                        ));*/
                        if (dataConnection.State == ConnectionState.Open)
                        {
                            dataConnection.Close();
                        }
                        dataConnection.ConnectionString = strConn;
                        dataConnection.Open();
                        icmd.Connection = dataConnection;
                        icmd.CommandTimeout = DataCmdTimeout;
                        icmd.CommandText = strProc;
                        icmd.CommandType = cmdType;
                        int i = arrPrm.GetUpperBound(0);
                        int j = 0;
                        icmd.Parameters.Clear();
                        foreach (MySqlParameter p in arrPrm)
                        {
                            if (j <= i)
                            {
                                icmd.Parameters.Add(p);
                            }
                            j++;
                        }
                        /*_logger.LogInformation(string.Format("{0} :: {1} :: create command end at {2}",
                           this.GetType().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           DateTime.Now
                        ));*/
                    }
                    /*_logger.LogInformation(string.Format("{0} :: {1} :: data connection end at {2}",
                        this.GetType().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        DateTime.Now
                    ));*/
                }
                tplObj = new Tuple<MySqlCommand, MySqlConnection>(icmd, dataConnection);
                /*_logger.LogInformation(string.Format("{0} :: {1} :: End at {2}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now
                ));*/

                if (tplObj.Item2.State == ConnectionState.Open)
                {
                    tplObj.Item2.Close();
                    tplObj.Item2.Dispose();
                }


                return tplObj;
               
            }
            catch (Exception err) {
                _logger.LogCritical(string.Format("{0} :: {1} :: Exception occured at {2} ERR:- {3}",
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    DateTime.Now,
                    err.Message
                ));
                return null;
            }
            
        }
        #endregion [Private Methods]
    }
}
