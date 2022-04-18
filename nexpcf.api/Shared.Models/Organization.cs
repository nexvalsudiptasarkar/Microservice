using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class Organization
    {
        public long OrgID { get; set; }
    }

    public class OrganizationList
    {
        public long OrgID { get; set; }
        public bool processExecute { get; set; }
    }

    public class OrganizationParam
    {
        public long in_orgid { get; set; }
    }
}
