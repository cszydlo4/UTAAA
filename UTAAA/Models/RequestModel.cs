using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class RequestModel
    {
        public int RequestID { get; set; }
        public string RequestDate { get; set; }
        public int RequestStatus { get; set; }
        public int ApprovalLevel { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string SECURITYACCESSDESC { get; set; }

    }
}