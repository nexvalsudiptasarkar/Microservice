using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class Users
    {
        public long UserID { get; set; }

    }

    public class UserRequest
    {
        public long UserID { get; set; }

        public DateTime UpdateTime { get; set; }

        public long OrgID { get; set; }

        public DateTime startDateTime { get; set; }

        public DateTime endDateTime { get; set; }

        public string Env { get; set; }

    }

    public class UsersParam
    {
        public long in_userid { get; set; }
    }

}
