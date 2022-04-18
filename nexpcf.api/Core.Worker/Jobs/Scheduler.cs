using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Worker.Jobs
{
    public class Scheduler
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Scheduler(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async void doSomething(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("updated do something running at: {time}", DateTimeOffset.Now);
                DataTable table = new DataTable();
                string query = @"insert into trialdotnet (entrytext) values (@entry)";
                string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
                MySqlDataReader myReader;
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myCommand.Parameters.AddWithValue("@entry", "ENTRY");

                        myReader = myCommand.ExecuteReader();

                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }
                await Task.Delay(3000, stoppingToken);
            }
        }

        public void doSomethingEvolve()
        {

            _logger.LogInformation("updated do something running at: {time}", DateTimeOffset.Now);
            DataTable table = new DataTable();
            string query = @"insert into trialdotnet (entrytext) values (@entry)";
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@entry", "ENTRY");

                    myReader = myCommand.ExecuteReader();

                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

        }

        /*public void doSomethingNew()
        {
            var obj = new Zone();
            try
            {
                using (SchedulerManager objManager = new SchedulerManager())
                {
                    obj = objManager.GetDetailsByID(new Zone() { ZoneUserID = username }, Constant.usp_zone);
                }
            }
            catch (Exception) { }
        }*/

        public void anything()
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                this.doSomethingEvolve();
            });
        }

        public async void secondaruDoSomething(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("updated daemon running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}