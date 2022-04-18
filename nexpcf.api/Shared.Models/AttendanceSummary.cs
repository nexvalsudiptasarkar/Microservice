using System;

namespace Shared.Models
{
    public class AttendanceSummary
    {
        public long in_userid { get; set; }
        public DateTime in_attendancedate { get; set; }
    }

    public class SummaryRef
    {
        public long objectid { get; set; }

        public int retrycount { get; set; }

        public int productid { get; set; }
        public int moduleid { get; set; }
        public int submoduleid { get; set; }

        public DateTime creationdate { get; set; }

        public DateTime modifieddate { get; set; }

        public int status { get; set; }
    }

    public class SummaryResult
    {
        public long attnid { get; set; }
        public string username { get; set; }
        public string empid { get; set; }
        public int? deptid { get; set; }
        public string? deptname { get; set; }
        public string emptype { get; set; }
        public int orgid { get; set; }
        public long userid { get; set; }
        public DateTime attndate { get; set; }
        public string attnstatus { get; set; }
        public string attnsubstatus { get; set; }
        public string halfday { get; set; }
        public string whichhalf { get; set; }
        public string iscoplus { get; set; }
        public string iscopplus { get; set; }
        public string iswopplus { get; set; }
        public string isphpplus { get; set; }
        public string doneshiftname { get; set; }
        public int? doneshiftid { get; set; }
        public string usershiftdetails { get; set; }
        public string userdefaultdayshift { get; set; }
        public string isholiday { get; set; }
        public string isweekoff { get; set; }
        public DateTime? checkintime { get; set; }
        public DateTime? checkouttime { get; set; }
        public string checkincheckoutpair { get; set; }
        public string checkinoutlocpair { get; set; }
        public string checkinoutlatlngpair { get; set; }
        public string checkoutpurposestr { get; set; }
        public string swpattnpair { get; set; }
        public string swplocpair { get; set; }
        public string swplatlngpair { get; set; }
        public int? totalpunchduration { get; set; }
        public int? totalswipedduration { get; set; }
        public int? totalworkedduration { get; set; }
        public int? totalbreakduration { get; set; }
        public int? totalothours { get; set; }
        public int? totallthours { get; set; }
        public int? userpayablewophours { get; set; }
        public int? userpayablephphours { get; set; }
        public string donemultipleshift { get; set; }
    }

    public class SummaryParameter
    {
        public long in_attnid { get; set; }
        public long attnid { get; set; }
        public string in_username { get; set; }
        public string in_empid { get; set; }
        public int? in_deptid { get; set; }
        public string? in_deptname { get; set; }
        public string in_emptype { get; set; }
        public int in_orgid { get; set; }
        public long in_userid { get; set; }
        public DateTime in_attndate { get; set; }
        public string in_attnstatus { get; set; }
        public string in_attnsubstatus { get; set; }
        public string in_halfday { get; set; }
        public string in_whichhalf { get; set; }
        public string in_iscoplus { get; set; }
        public string in_iscopplus { get; set; }
        public string in_iswopplus { get; set; }
        public string in_isphpplus { get; set; }
        public string in_doneshiftname { get; set; }
        public int? in_doneshiftid { get; set; }
        public string in_usershiftdetails { get; set; }
        public string in_userdefaultdayshift { get; set; }
        public string in_isholiday { get; set; }
        public string in_isweekoff { get; set; }
        public DateTime? in_checkintime { get; set; }
        public DateTime? in_checkouttime { get; set; }
        public string in_checkincheckoutpair { get; set; }
        public string in_checkinoutlocpair { get; set; }
        public string in_checkinoutlatlngpair { get; set; }
        public string in_checkoutpurposestr { get; set; }
        public string in_swpattnpair { get; set; }
        public string in_swplocpair { get; set; }
        public string in_swplatlngpair { get; set; }
        public int? in_totalpunchduration { get; set; }
        public int? in_totalswipedduration { get; set; }
        public int? in_totalworkedduration { get; set; }
        public int? in_totalbreakduration { get; set; }
        public int? in_totalothours { get; set; }
        public int? in_totallthours { get; set; }
        public int? in_userpayablewophours { get; set; }
        public int? in_userpayablephphours { get; set; }
        public string in_donemultipleshift { get; set; }
    }
}

