using System;
using ObjectExtensions;


namespace Shared.Data
{
    public sealed partial class PWConnectionStringleton : Singleton<PWConnectionStringleton>
    {
        public PWConnectionString PWConnString { get; set; }

        public void Init(string name, string server, string database, bool encryptTraffic = false, string uid = "", string pwd = "")
        {
            this.PWConnString = new PWConnectionString(name, server, database, encryptTraffic, uid, pwd);
        }
    }

    public class PWConnectionString
    {
        public bool EncryptTraffic { get; set; }
        public string Name { get; set; }
        protected string Server { get; set; }
        protected string Database { get; set; }
        protected string UserID { get; set; }
        protected string Password { get; set; }
        private bool IsValid
        {
            get
            {
                string value = string.Concat(this.Server, ",", this.Database, ",", this.UserID, ",", this.Password);
                string[] parts = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return (parts.Length >= 2);
            }
        }
        private string Credentials
        {
            get
            {
                string value = "Integrated Security=";
                if (string.IsNullOrEmpty(this.Password) && string.IsNullOrEmpty(this.UserID))
                {
                    value = string.Format("{0}true;", value);
                }
                else
                {
                    value = string.Format("{0}false; user id={1}; password={2};", value, this.UserID, this.Password);
                }
                return value;
            }
        }
        private string WithEncryptedTraffic
        {
            get
            {
                string value = string.Empty;
                if (this.EncryptTraffic)
                {
                    value = "Encrypt=true; TrustServerCertificate=true;";
                }
                return value;
            }
        }
        public string ConnectionString
        {
            get
            {
                string value = string.Empty;
                if (this.IsValid)
                {
                    value = string.Format("data source={0}; initial catalog={1}; {2} {3}", this.Server, this.Database, this.Credentials, this.WithEncryptedTraffic);
                }
                else
                {
                    throw new InvalidOperationException("One or more required connection string parameters were not specified.");
                }
                return value.Base64Encode();
            }
        }

        public PWConnectionString(string name, string server, string database, bool encryptTraffic = false, string uid = "", string pwd = "")
        {
            this.Name = name;
            if (string.IsNullOrEmpty(server)) { throw new ArgumentNullException("server"); }
            if (string.IsNullOrEmpty(database)) { throw new ArgumentNullException("database"); }
            this.Server = server;
            this.Database = database;
            this.UserID = uid;
            this.Password = pwd;
            this.EncryptTraffic = encryptTraffic;
        }

        public override string ToString()
        {
            return this.ConnectionString;
        }
    }
}
