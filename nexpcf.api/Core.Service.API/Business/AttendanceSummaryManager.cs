using Core.Producer.API.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Shared.Data;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Producer.API.Business
{
    public class AttendanceSummaryManager : Disposable
    {
        private readonly ILogger<AttendanceSummaryManager> _logger;
        private MySqlDataHelper sqldatahelper = new MySqlDataHelper();

        public AttendanceSummaryManager(ILogger<AttendanceSummaryManager> logger)
        {
            _logger = logger;
        }

        protected override void cleanup() { }
        public async Task generateAttendanceSummaryAutoProcess()
        {
            _logger.LogInformation(string.Format("{0} :: {1} :: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Info log"));

            try
            {
                DateTime fromdate = new DateTime(2021, 01, 01);
                DateTime todate = new DateTime(2021, 01, 31);
                DateTime startdatetmp = new DateTime();
                startdatetmp = fromdate;
                string env = "PROD";
                while (startdatetmp <= todate)
                {
                    try
                    {
                        _logger.LogInformation(string.Format("{0} :: {1} :: Data generation started for date {2} at {3}",
                            this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                            startdatetmp, DateTime.Now));
                        AttendanceSummary obj = new AttendanceSummary();

                        MySqlParameter[] parameter = Util.GetParameter(obj);
                        _logger.LogInformation(string.Format("{0} :: {1} :: Fetching org list started at {2}",
                            this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                            DateTime.Now));
                        var orgobject = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_get_orgs, CommandType.StoredProcedure, parameter, _logger).ToListAsync<Organization>();
                        _logger.LogInformation(string.Format("{0} :: {1} :: Fetching org list ended at {2}",
                            this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                            DateTime.Now));
                        List<Organization> orglist = new List<Organization>();
                        orglist.AddRange(orgobject);

                        foreach (var eachorgobj in orglist)
                        {
                            OrganizationParam objorguser = new OrganizationParam();
                            objorguser.in_orgid = eachorgobj.OrgID;
                            MySqlParameter[] parameteruser = Util.GetParameter(objorguser);

                            _logger.LogInformation("{0} :: {1} :: ------ORG START-----", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                            _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing for {2} start for date {3} at {4}",
                                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                                objorguser.in_orgid, startdatetmp, DateTime.Now));

                            _logger.LogInformation(string.Format("{0} :: {1} :: Fetching userlist of org {2} started at {3}",
                                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                    , objorguser.in_orgid, DateTime.Now));
                            var orguserlistobj = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_get_orgs_users, CommandType.StoredProcedure, parameteruser, _logger).ToListAsync<Users>();
                            _logger.LogInformation(string.Format("{0} :: {1} :: Fetching userlist of org {2} ended at {3}",
                                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                    , objorguser.in_orgid, DateTime.Now));
                            List<Users> userlist = new List<Users>();
                            userlist.AddRange(orguserlistobj);
                            //foreach (var eachUserObj in userlist)
                            await userlist.ParallelForEachAsync(async (eachUserObj) =>
                            {
                                AttendanceSummary objatten = new AttendanceSummary();
                                objatten.in_attendancedate = startdatetmp;
                                objatten.in_userid = eachUserObj.UserID;
                                try
                                {
                                    MySqlParameter[] parameterAttn = Util.GetParameter(objatten);

                                    _logger.LogInformation("-------------");
                                    _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing for {2} of org {3} start for date {4} at {5}",
                                    this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                        , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));


                                    _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing Read Data for {2} of org {3} started for date {4} at {5}",
                                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                    , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));
                                    var resultSummary = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_generate_attendance_status, CommandType.StoredProcedure, parameterAttn, _logger).ToListAsync<SummaryResult>();
                                    _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing Read Data for {2} of org {3} ended for date {4} at {5}",
                                    this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                        , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));


                                    List<SummaryResult> resultSummaryLstObj = new List<SummaryResult>();
                                    resultSummaryLstObj.AddRange(resultSummary);

                                    SummaryResult summaryObj = resultSummaryLstObj[0];
                                    if (summaryObj.attnstatus != "")
                                    {
                                        SummaryParameter param = new SummaryParameter();

                                        Util.CopyPropertiesFrom(param, summaryObj);

                                        MySqlParameter[] parameterAttnSummary = Util.GetParameter(param);
                                        _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing updating or inserting Data for {2} of org {3} started for date {4} at {5}",
                                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                            , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));
                                        await this.sqldatahelper.ExecuteNonQuery(Util.CS(env), Constant.nex_save_attendance_status, CommandType.StoredProcedure, parameterAttnSummary, _logger);
                                        _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing updating or inserting Data for {2} of org {3} ended for date {4} at {5}",
                                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                            , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));
                                    }

                                    _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing for {2} of org {3} end for date {4} at {5}",
                                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                    , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));

                                    _logger.LogInformation(string.Format("{0} :: {1} :: Data successfully processed for {2} of org {3} for date {4} at {5}",
                                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                    , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));
                                    _logger.LogInformation("-------------");
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(string.Format("{0} :: {1} :: Exception occured for {2} of org {3} for date {4} at {5}",
                                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                    , objatten.in_userid, objorguser.in_orgid, startdatetmp, DateTime.Now));
                                }
                            }, maxDegreeOfParallelism: 5);
                            _logger.LogInformation(string.Format("{0} :: {1} :: Attendance Processing for {2} end for date {3} at {4}",
                                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                                , objorguser.in_orgid, startdatetmp, DateTime.Now));
                            _logger.LogInformation("{0} :: {1} :: ------ORG END-----", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        }
                        _logger.LogInformation(string.Format("{0} :: {1} :: Data generation ended for date {2} at {3}",
                            this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                            startdatetmp, DateTime.Now));

                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical(string.Format("{0} :: {1} :: Data generation Exception occured for date {2} at {3} Err:- {4}",
                        this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                            , startdatetmp, DateTime.Now, e.Message));
                    }
                    finally
                    {
                        startdatetmp = startdatetmp.AddDays(1);
                    }

                }

            }
            catch (Exception e)
            {
                _logger.LogCritical(string.Format("{0} :: {1} :: Main exception occured at {2}",
                    this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                , DateTime.Now));
                //_logger.LogInformation("GenerateAttendanceSummary failed: {time}", DateTimeOffset.Now);
                throw e;
            }
        }
    }
}
