﻿using Core.Consumer.API.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Shared.Data;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Consumer.API.Business
{
    class EarlyCheckOutManager : Disposable
    {
        private MySqlDataHelper sqldatahelper = new MySqlDataHelper();
        //private readonly ILogger<Worker> _logger;
        public MySqlConnection DataConn = new MySqlConnection();
        protected override void cleanup() { }
        private readonly ILogger _logger;

        public EarlyCheckOutManager(ILogger logger)
        {
            _logger = logger;
        }
        public EarlyCheckOutManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public async Task processEarlyCheckOut()
        {
            try
            {
                string env = "PROD";

                EarlyCheckOut obj = new EarlyCheckOut();
                AttendanceSummary objattn = new AttendanceSummary();
                obj.in_attendancedate = new DateTime(2022,02,11).AddDays(-2);
                objattn.in_attendancedate = new DateTime(2022, 02, 11).AddDays(-2);
                objattn.in_userid = 0;
                MySqlParameter[] parameter = Util.GetParameter(objattn);


                /*_logger.LogInformation(string.Format("{0} :: {1} :: Fetching org list started at {2}",
                    this.GetType().Name, "processEarlyCheckOut",
                    DateTime.Now));*/

                var orgobject = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_get_orgs, CommandType.StoredProcedure, parameter, _logger).ToListAsync<Organization>();

                /*_logger.LogInformation(string.Format("{0} :: {1} :: Fetching org list ended at {2}",
                    this.GetType().Name, "processEarlyCheckOut",
                    DateTime.Now));*/

                List<Organization> orglist = new List<Organization>();
                orglist.AddRange(orgobject);


                foreach (var eachorgobj in orglist)
                {
                    try {
                        obj.in_orgid = 746;
                        MySqlParameter[] objParam = Util.GetParameter(obj);
                        _logger.LogInformation(string.Format("{0} :: {1} :: Getting all early checkouts started at {2}",
                        this.GetType().Name, "processEarlyCheckOut",
                        DateTime.Now));
                        var resultEarlyCheckout = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_process_early_checkout, CommandType.StoredProcedure, objParam, _logger).ToListAsync<EarlyCheckOutResult>();
                        List<EarlyCheckOutResult> earlyCheckoutListObj = new List<EarlyCheckOutResult>();
                        _logger.LogInformation(string.Format("{0} :: {1} :: Getting all early checkouts ended at {2}",
                        this.GetType().Name, "processEarlyCheckOut",
                        DateTime.Now));
                        earlyCheckoutListObj.AddRange(resultEarlyCheckout);
                        foreach (var eachearlycheckoutobj in earlyCheckoutListObj) {
                            try
                            {
                                EarlyCheckOutResult earlycheckoutobj = eachearlycheckoutobj;

                                EarlyCheckOutResultParameter param = new EarlyCheckOutResultParameter();
                                Util.CopyPropertiesFrom(param, earlycheckoutobj);

                                MySqlParameter[] paramEarlyCheckOut = Util.GetParameter(param);
                                _logger.LogInformation(string.Format("{0} :: {1} ::Data saving started at {2}",
                                this.GetType().Name, "processEarlyCheckOut",
                                DateTime.Now));
                                await this.sqldatahelper.ExecuteNonQuery(Util.CS(env), Constant.nex_save_early_checkouts, CommandType.StoredProcedure, paramEarlyCheckOut, _logger);
                                _logger.LogInformation(string.Format("{0} :: {1} ::Data saving ended at {2}",
                                this.GetType().Name, "processEarlyCheckOut",
                                DateTime.Now));
                            }
                            catch (Exception e) {
                                _logger.LogCritical(string.Format("{0} :: {1} :: exception occured for org in earlycheckout object loop {2} at {3} :: Err:- {4}",
                                this.GetType().Name, "processEarlyCheckOut", eachorgobj.OrgID
                                , DateTime.Now, e.Message));
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        _logger.LogCritical(string.Format("{0} :: {1} :: EarlyCheckOutManager {2} at {3} :: Err:- {4}",
                        this.GetType().Name, "processEarlyCheckOut", eachorgobj.OrgID
                        , DateTime.Now, e.Message));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(string.Format("{0} :: {1} :: Main exception occured at {2} :: Err:- {3}",
                    this.GetType().Name, "processEarlyCheckOut"
                , DateTime.Now, e.Message));
                throw e;
            }
        }
    }
}
