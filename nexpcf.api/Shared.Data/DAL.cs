using ObjectExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared.Data
{
    public class EntityRowCount
    {
        public int Count { get; set; }
    }

    public partial class DAL
    {

        #region properties
        public bool FailOnMismatch { get; set; }
        public int TimeoutSecs { get; set; }
        public string ConnectionString { get; protected set; }
        public bool AddReturnParamIfMissing { get; set; }
        public int BulkInsertBatchSize { get; set; }
        protected int BulkCopyTimeout { get; set; }
        //protected MySqlBulkCopyOptions BulkCopyOptions { get; set; }
        protected MySqlTransaction ExternalTransaction { get; set; }
        public bool CanReadWriteDB { get; set; }
        #endregion properties

        #region constructors
        public DAL(string connStr)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                throw new ArgumentNullException("connStr");
            }
            this.ConnectionString = connStr;
            this.Init();
        }

        public DAL(PWConnectionString connStr) : this(connStr.ConnectionString)
        {
        }

        #endregion constructors

        #region protected data access methods
        protected virtual List<T> ExecuteStoredProc<T>(string storedProc, MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(storedProc))
            {
                throw new ArgumentNullException("storedProc");
            }
            DataTable data = this.GetData(storedProc, parameters, CommandType.StoredProcedure);
            List<T> collection = this.MakeEntityFromDataTable<T>(data);
            return collection;
        }

        protected virtual int ExecuteStoredProc(string storedProc, MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(storedProc))
            {
                throw new ArgumentNullException("storedProc");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (parameters.Length == 0)
            {
                throw new InvalidOperationException("The [parameters] array must contain at least one item.");
            }
            int result = this.SetData(storedProc, parameters, CommandType.StoredProcedure);
            return result;
        }

        protected virtual int ExecuteStoredProc<T>(T data, string storedProc, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string paramArrayPropName = "MySqlParameters")
        {
            int result = 0;
            MySqlParameter[] parameters = DAL.MakeSqlParameters(data, bulkType, precedence, paramArrayPropName);
            result = this.ExecuteStoredProc(storedProc, parameters);
            return result;
        }

        protected virtual int ExecuteStoredProc<T>(IEnumerable<T> data, string storedProc, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string paramArrayPropName = "MySqlParameters")
        {
            if (string.IsNullOrEmpty(storedProc))
            {
                throw new ArgumentNullException("storedProc");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count() == 0)
            {
                throw new InvalidOperationException("Data collection must contain at least onme item");
            }

            int result = this.DoBulkMerge(data, storedProc, bulkType, CommandType.StoredProcedure, precedence, true);
            return result;
        }

        protected virtual List<T> ExecuteQuery<T>(string query, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }
            DataTable data = this.GetData(query, parameters, CommandType.Text);
            List<T> collection = this.MakeEntityFromDataTable<T>(data);
            return collection;
        }

        protected virtual int ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }
            int result = this.SetData(query, parameters, CommandType.Text);
            return result;
        }

        protected virtual int ExecuteQuery<T>(T data, string query, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string paramArrayPropName = "MySqlParameters")
        {
            int result = 0;
            MySqlParameter[] parameters = DAL.MakeSqlParameters(data, bulkType);
            result = this.ExecuteQuery(query, parameters);
            return result;
        }

        protected virtual int ExecuteQuery<T>(IEnumerable<T> data, string query, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string paramArrayPropName = "MySqlParameters")
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count() == 0)
            {
                throw new InvalidOperationException("Data collection must contain at least onme item");
            }
            int result = this.DoBulkMerge(data, query, bulkType, CommandType.Text, precedence, true);
            return result;
        }

        /*protected virtual int ExecuteBulkInsert(DataTable dataTable)
        {
            if (string.IsNullOrEmpty(dataTable.TableName))
            {
                throw new InvalidOperationException("The table name MUST be specified in the datatable (including the schema).");
            }
            if (!dataTable.TableName.Contains('.') || dataTable.TableName.StartsWith("."))
            {
                throw new InvalidOperationException("The schema MUST be specified with the table name.");
            }
            if (dataTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("The dataTable must contain at least one item");
            }

            int recsBefore = this.BulkInsertTargetCount(dataTable.TableName);

            int recordsAffected = 0;
            MySqlConnection conn = null;
            MySqlConnector.MySqlBulkCopy bulk = null;

            using (conn = new MySqlConnection(this.ConnectionString.Base64Decode()))
            {
                conn.Open();
                bulk = new MySqlConnector.MySqlBulkCopy((MySqlConnector.MySqlConnection)conn, (MySqlConnector.MySqlConnection)this.ExternalTransaction));
                Debug.WriteLine("DoBulkInsert - inserting {0} rows", dataTable.Rows.Count);
                bulk.WriteToServer(dataTable);
            }

            int recsAfter = this.BulkInsertTargetCount(dataTable.TableName);
            recordsAffected = recsAfter - recsBefore;
            return recordsAffected;
        }

        protected virtual int ExecuteBulkInsert<T>(IEnumerable<T> data, string tableName, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string paramArrayPropName = "MySqlParameters")
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.Count() == 0)
            {
                throw new InvalidOperationException("The data collection must contain at least one item");
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("The tableName parameter cannot be null or empty.");
            }
            if (!tableName.Contains('.'))
            {
                throw new InvalidOperationException("The schema MUST be specified with the table name.");
            }

            int result = 0;
            DataTable dataTable = null;

            if (data.Count() > 0)
            {
                dataTable = this.MakeDataTable(data, tableName, bulkType, precedence, paramArrayPropName);
                result = this.ExecuteBulkInsert(dataTable);
            }
            return result;
        }*/

        #endregion protected data access methods

        #region Protected helper methods

        protected virtual void Init()
        {
            this.TimeoutSecs = 300;
            this.FailOnMismatch = false;
            this.AddReturnParamIfMissing = true;
            this.ExternalTransaction = null;
            this.BulkInsertBatchSize = 250;
            this.BulkCopyTimeout = 600;
            //this.BulkCopyOptions = MySqlBulkCopyOptions.Default;
            this.CanReadWriteDB = true;
        }

        protected virtual string TestConnection()
        {
            string result = string.Empty;
            MySqlConnection conn = null;
            try
            {
                using (conn = new MySqlConnection(this.ConnectionString.Base64Decode()))
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        protected virtual string AddTryCatchTranToQuery(string query, string logQuery, string transactionName = "")
        {
            transactionName = transactionName.Trim();
            logQuery = logQuery.Trim();

            StringBuilder text = new StringBuilder();
            text.AppendLine("BEGIN TRY");
            text.AppendFormat("    BEGIN TRAN {0};", transactionName).AppendLine();
            text.AppendLine(query).AppendLine();
            text.AppendFormat("    COMMIT TRAN {0};", transactionName).AppendLine();
            text.AppendLine("END TRY");
            text.AppendLine("BEGIN CATCH");
            text.AppendFormat("    IF @@TRANCOUNT > 0 ROLLBACK TRAN {0};", transactionName).AppendLine();
            text.AppendLine(logQuery);
            text.AppendLine("END CATCH");
            return text.ToString();
        }

        protected virtual string NormalizeTableName(string tableName)
        {
            string[] parts = tableName.Split('.');
            tableName = string.Empty;
            foreach (string part in parts)
            {
                tableName = (string.IsNullOrEmpty(tableName))
                            ? string.Format("[{0}]", part)
                            : string.Format(".[{0}]", part);
            }
            return tableName.Replace("[[", "[").Replace("]]", "]");
        }

        protected virtual int BulkInsertTargetCount(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (!tableName.Contains('.') || tableName.StartsWith("."))
            {
                throw new InvalidOperationException("The [tableName] must include a schema. Example: 'dbo.tableName'");
            }

            int result = 0;
            string query = string.Format("SELECT COUNT(1) FROM {0}", this.NormalizeTableName(tableName));
            List<EntityRowCount> rowCount = this.ExecuteQuery<EntityRowCount>(query);
            if (rowCount != null && rowCount.Count > 0)
            {
                result = rowCount[0].Count;
            }
            return result;
        }

        protected virtual DataTable GetData(string cmdText, MySqlParameter[] parameters = null, CommandType cmdType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException("cmdText");
            }

            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            DataTable data = null;

            using (conn = new MySqlConnection(this.ConnectionString.Base64Decode()))
            {
                conn.Open();
                using (cmd = new MySqlCommand(cmdText, conn) { CommandTimeout = this.TimeoutSecs, CommandType = cmdType })
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    if (this.CanReadWriteDB)
                    {
                        using (reader = cmd.ExecuteReader())
                        {
                            data = new DataTable();
                            data.Load(reader);
                        }
                    }
                }
            }
            return data;
        }

        protected virtual int SetData(string cmdText, MySqlParameter[] parameters, CommandType cmdType = CommandType.StoredProcedure, bool useAdoTransaction = false)
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException("cmdText");
            }

            int result = 0;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlTransaction transaction = null;
            using (conn = new MySqlConnection(this.ConnectionString.Base64Decode()))
            {
                conn.Open();
                if (useAdoTransaction && cmdType != CommandType.StoredProcedure)
                {
                    transaction = conn.BeginTransaction();
                }

                using (cmd = new MySqlCommand(cmdText, conn) { CommandTimeout = this.TimeoutSecs, CommandType = cmdType })
                {
                    MySqlParameter rowsAffected = null;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                        if (cmdType == CommandType.StoredProcedure && this.AddReturnParamIfMissing)
                        {
                            rowsAffected = parameters.FirstOrDefault(x => x.Direction == ParameterDirection.ReturnValue);
                            if (rowsAffected == null)
                            {
                                rowsAffected = cmd.Parameters.Add(new MySqlParameter("@rowsAffected", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                            }
                        }
                    }
                    try
                    {
                        if (this.CanReadWriteDB)
                        {
                            result = cmd.ExecuteNonQuery();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        if (transaction != null && cmdType != CommandType.StoredProcedure)
                        {
                            transaction.Rollback();
                        }
                        throw (ex);
                    }
                    result = (rowsAffected != null) ? (int)rowsAffected.Value : result;
                }
            }
            return result;
        }

        protected virtual int DoBulkMerge<T>(IEnumerable<T> data, string queryText, BulkInsertType bulkType, CommandType cmdType, ParamPrecedence precedence = ParamPrecedence.None, bool useAdoTransaction = false)
        {
            if (string.IsNullOrEmpty(queryText))
            {
                throw new ArgumentNullException("queryText");
            }

            int result = 0;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlTransaction transaction = null;
            using (conn = new MySqlConnection(this.ConnectionString.Base64Decode()))
            {
                conn.Open();
                if (useAdoTransaction && cmdType != CommandType.StoredProcedure)
                {
                    transaction = conn.BeginTransaction();
                }
                using (cmd = new MySqlCommand(queryText, conn) { CommandTimeout = this.TimeoutSecs, CommandType = cmdType })
                {
                    try
                    {
                        foreach (T item in data)
                        {
                            var paramArrayPropName = "MySqlParameters";
                            MySqlParameter[] parameters = DAL.MakeSqlParameters(item, bulkType, precedence, paramArrayPropName);
                            if (parameters != null)
                            {
                                cmd.Parameters.AddRange(parameters);
                                if (this.CanReadWriteDB)
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                cmd.Parameters.Clear();
                                result++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        throw (ex);
                    }
                }
            }
            return result;
        }

        protected static T ConvertFromDBValue<T>(object obj, T defaultValue)
        {
            T result = (obj == null || obj == DBNull.Value) ? default(T) : (T)obj;
            return result;
        }

        protected virtual List<T> MakeEntityFromDataTable<T>(DataTable data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Type objType = typeof(T);
            List<T> collection = new List<T>();
            if (data != null && data.Rows.Count > 0)
            {
                int matched = 0;

                foreach (DataRow row in data.Rows)
                {
                    T item = (T)Activator.CreateInstance(objType);

                    PropertyInfo[] properties = objType.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        if (data.Columns.Contains(property.Name))
                        {
                            Type pType = property.PropertyType;
                            var defaultValue = pType.GetDefaultValue();
                            var value = row[property.Name];
                            value = DAL.ConvertFromDBValue(value, defaultValue);
                            property.SetValue(item, value);
                            matched++;
                        }
                    }
                    if (matched != data.Columns.Count && this.FailOnMismatch)
                    {
                        throw new Exception("Data retrieved does not match specified model.");
                    }
                    collection.Add(item);
                }
            }
            return collection;
        }

        protected virtual DataTable MakeDataTable<T>(IEnumerable<T> data, string tableName, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string paramArrayPropName = "MySqlParameters")
        {
            DataTable dataTable = null;

            Debug.WriteLine(string.Format("MakeDataTable - {0} data items specified.", data.Count()));

            using (dataTable = new DataTable() { TableName = tableName })
            {
                Type type = typeof(T);

                PropertyInfo[] properties = DAL.GetEntityProperties(type, bulkType);

                Debug.WriteLine(string.Format("MakeDataTable - {0} item properties per item.", properties.Length));

                foreach (PropertyInfo property in properties)
                {
                    dataTable.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                }

                Debug.WriteLine(string.Format("MakeDataTable - {0} dataTable columns created.", dataTable.Columns.Count));

                foreach (T entity in data)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (PropertyInfo property in properties)
                    {
                        row[property.Name] = property.GetValue(entity);
                    }
                    dataTable.Rows.Add(row);
                }
            }

            Debug.WriteLine(string.Format("MakeDataTable - {0} rows created.", dataTable.Rows.Count));

            return dataTable;
        }

        #endregion Protected helper methods

    }
}
