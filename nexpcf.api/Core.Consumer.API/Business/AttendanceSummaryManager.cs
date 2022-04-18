using Core.Consumer.API.Consumers;
using Core.Consumer.API.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Nexval.Framework.PCF;
using Nexval.Framework.PCF.Threading;
using Shared.Data;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;


namespace Core.Consumer.API.Business
{
    public class AttendanceSummaryManager : Disposable
    {
        private MySqlDataHelper sqldatahelper = new MySqlDataHelper();
        //private readonly ILogger<Worker> _logger;
        public MySqlConnection DataConn = new MySqlConnection();

        private ITaskManager<UserRequest> _taskManager = null;

        private readonly ILogger _logger;
        protected override void cleanup() { }

        public AttendanceSummaryManager(ILogger logger)
        {
            _logger = logger;
        }

        public AttendanceSummaryManager(IConfiguration configuration, ILogger logger)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task generateAttendanceSummary(SummaryRef obj)
        {
            try
            {
                String env = "UAT";
                MySqlParameter[] parameterAttn = Util.GetParameter(obj);

                //_logger.LogInformation("Attendance Summary started for {user} of {org} at {time}", eachUserObj.UserID, eachorgobj.OrgID, DateTimeOffset.Now);
                if (obj.objectid > 0)
                {

                    var resultSummary = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_re_gen_delta_modified_data, CommandType.StoredProcedure, parameterAttn, _logger).ToListAsync<SummaryResult>();
                    List<SummaryResult> resultSummaryLstObj = new List<SummaryResult>();
                    resultSummaryLstObj.AddRange(resultSummary);

                    SummaryResult summaryObj = resultSummaryLstObj[0];

                    SummaryParameter param = new SummaryParameter();

                    Util.CopyPropertiesFrom(param, summaryObj);

                    MySqlParameter[] parameterAttnSummary = Util.GetParameter(param);

                    await this.sqldatahelper.ExecuteNonQuery(Util.CS(env), Constant.nex_save_attendance_status, CommandType.StoredProcedure, parameterAttnSummary, _logger);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("{0} :: {1} :: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "GenerateAttendanceSummary failed: {time}"));
                throw e;
            }
        }
        public async Task generateAttendanceSummaryAutoProcess()
        {

            try
            {
                DateTime todate = new DateTime(2021, 02, 15);
                DateTime startdatetmp = new DateTime();
                DateTime fromdate = new DateTime(2021, 02, 01);
                startdatetmp = fromdate;
                string env = "PROD";
                while (startdatetmp <= todate)
                {
                    try
                    {
                        AttendanceSummary obj = new AttendanceSummary();
                        MySqlParameter[] parameter = Util.GetParameter(obj);
                        var orgobject = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_get_orgs, CommandType.StoredProcedure, parameter, _logger).ToListAsync<Organization>();
                        List<Organization> orglist = new List<Organization>();
                        orglist.AddRange(orgobject);

                        foreach (var eachorgobj in orglist)
                        {
                            OrganizationParam objorguser = new OrganizationParam();
                            objorguser.in_orgid = eachorgobj.OrgID;
                            MySqlParameter[] parameteruser = Util.GetParameter(objorguser);

                            var orguserlistobj = await this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_get_orgs_users, CommandType.StoredProcedure, parameteruser, _logger).ToListAsync<Users>();
                            
                            //List<UserRequest> userlist = new List<UserRequest>();
                            //userlist.AddRange(orguserlistobj);
                            //customPool.QueueUserTask(() =>
                            //{
                            //    userlist.ForEach(async eachUserObj =>
                            //    {
                            //        await this.processAttendanceDataPerUser(eachUserObj, startdatetmp, env, objorguser.in_orgid);
                            //    });
                            //},
                            //(ts) =>
                            //{



                            //});

                            initialize(orguserlistobj, objorguser.in_orgid, fromdate, 2,true);



                        }

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
                throw e;
            }
        }
        private async Task<int> processAttendanceDataPerUser(UserRequest userData, DateTime startdatetmp, string env, long in_orgid)
        {
            int result = default(int);
            AttendanceSummary objatten = new AttendanceSummary();
            objatten.in_attendancedate = startdatetmp;
            objatten.in_userid = userData.UserID;
            try
            {
                MySqlParameter[] parameterAttn = Util.GetParameter(objatten);



                var resultSummary = this.sqldatahelper.ExecuteDataSet(Util.CS(env), Constant.nex_generate_attendance_status, CommandType.StoredProcedure, parameterAttn, _logger).ToListAsync<SummaryResult>().Result;

                List<SummaryResult> resultSummaryLstObj = new List<SummaryResult>();
                resultSummaryLstObj.AddRange(resultSummary);

                SummaryResult summaryObj = resultSummaryLstObj[0];
                if (summaryObj.attnstatus != "")
                {
                    SummaryParameter param = new SummaryParameter();
                    Util.CopyPropertiesFrom(param, summaryObj);
                    MySqlParameter[] parameterAttnSummary = Util.GetParameter(param);
                    return this.sqldatahelper.ExecuteNonQuery(Util.CS(env), Constant.nex_save_attendance_status, CommandType.StoredProcedure, parameterAttnSummary, _logger).Result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("{0} :: {1} :: Exception occured for {2} of org {3} for date {4} at {5}",
                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                , objatten.in_userid, in_orgid, startdatetmp, DateTime.Now));
            }
            return default(int);
        }
        private int processAttendanceDataPerUserSync(UserRequest userData)
        {
            int result = default(int);
            AttendanceSummary objatten = new AttendanceSummary();
            objatten.in_attendancedate = userData.startDateTime;
            objatten.in_userid = userData.UserID;
            try
            {
                MySqlParameter[] parameterAttn = Util.GetParameter(objatten);



                var resultSummary = this.sqldatahelper.ExecuteDataSet(Util.CS(userData.Env), Constant.nex_generate_attendance_status, CommandType.StoredProcedure, parameterAttn, _logger).ToListAsync<SummaryResult>().Result;

                List<SummaryResult> resultSummaryLstObj = new List<SummaryResult>();
                resultSummaryLstObj.AddRange(resultSummary);

                SummaryResult summaryObj = resultSummaryLstObj[0];
                if (summaryObj.attnstatus != "")
                {
                    SummaryParameter param = new SummaryParameter();
                    Util.CopyPropertiesFrom(param, summaryObj);
                    MySqlParameter[] parameterAttnSummary = Util.GetParameter(param);
                    return this.sqldatahelper.ExecuteNonQuery(Util.CS(userData.Env), Constant.nex_save_attendance_status, CommandType.StoredProcedure, parameterAttnSummary, _logger).Result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("{0} :: {1} :: Exception occured for {2} of org {3} for date {4} at {5}",
                this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name
                , objatten.in_userid, userData.OrgID, userData.startDateTime, DateTime.Now));
            }
            return default(int);
        }
        private bool getUserInputs(ref int concurrency, ref bool enableAutoStop)
        {
            enableAutoStop = true;
            concurrency = 2;

            return false;

        }
        private void initialize(List<Users> requestList,long OrgId,DateTime startDateTime, int concurrency, bool enableAutoStop)
        {
            stopTaskManager();

            _taskManager = NexvalPcfFactory.GetTaskManager<UserRequest>(executeRequests, enableAutoStop, "Attendance-Summary", concurrency, 3);
            _taskManager.OnTaskExecFailed += OnTaskExecFailed;
            _taskManager.OnTaskExecSuccessful += OnTaskExecSuccessful;
            _taskManager.OnStopped += onTaskManagerStopped;

            generateAndEnqueueNewRequests(requestList, OrgId,startDateTime);
            _taskManager.Start();
        }
        private void stopTaskManager()
        {
            if (_taskManager != null)
            {
                Console.WriteLine("An Task Manager is active! Would you like to stop this & create a new one?" + "Question");
                _taskManager.OnTaskExecFailed -= OnTaskExecFailed;
                _taskManager.OnTaskExecSuccessful -= OnTaskExecSuccessful;
                _taskManager.Stop();
                _taskManager.Dispose();
            }
        }
        private void OnTaskExecFailed(ITaskManager<UserRequest> source, long requestTrackingId, UserRequest request, int retryCount, bool isPermanentlyFailed, ref bool shouldTaskManagerBeTerminated)
        {
            Console.WriteLine("Exec Failed for Task Id:{0}, Data:{1}, Retry Count:{2}!", requestTrackingId, request, retryCount);
        }
        private void OnTaskExecSuccessful(ITaskManager<UserRequest> source, long requestTrackingId, UserRequest request)
        {
            Console.WriteLine("Exec Successful for Task Id:{0}, Data:{1}.", requestTrackingId, request);

            request.UpdateTime = DateTime.Now;

        }

        private void onTaskManagerStopped(ITaskManager<UserRequest> source)
        {
            _taskManager.OnStopped -= onTaskManagerStopped;
        }
        private bool executeRequests(long trackingId, UserRequest request)
        {
            processAttendanceDataPerUserSync(request);
            return true;
        }

        private void generateAndEnqueueNewRequests(List<Users> requestList,long OrgId,DateTime startDateTime)
        {

            var requestArray = new List<UserRequest>();

            foreach(Users usr in requestList)
            {
                UserRequest req = new UserRequest();
                req.UserID = usr.UserID;
                req.OrgID = OrgId;
                req.startDateTime = startDateTime;
                requestArray.Add(req);
            }

            long[] ids = _taskManager.EnqueueMultiple(requestArray.ToArray());

        }
    }
}
