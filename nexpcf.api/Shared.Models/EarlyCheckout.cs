using System;

namespace Shared.Models
{
    public class EarlyCheckOut
    {
        public long in_orgid { get; set; }
        public DateTime in_attendancedate { get; set; }
    }

    public class EarlyCheckOutResult {
		public long orgid { get; set; }
		public long attnid { get; set; }
		public long userid { get; set; }
		public string attntype { get; set; }
		public DateTime attndatetime { get; set; }
		public int workedduration { get; set; }
		public int lessduration { get; set; }
		public long suoervisorid { get; set; }
		public string req_status { get; set; }
		public string isoutdated { get; set; }
	}

	public class EarlyCheckOutResultParameter
	{
		public long in_orgid { get; set; }
		public long in_attnid { get; set; }
		public long in_userid { get; set; }
		public string in_attntype { get; set; }
		public DateTime in_attndatetime { get; set; }
		public int in_workedduration { get; set; }
		public int in_lessduration { get; set; }
		public long in_suoervisorid { get; set; }
		public string in_req_status { get; set; }
		public string in_isoutdated { get; set; }
	}

}

