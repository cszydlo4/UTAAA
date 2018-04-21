using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class RequestModel
    {
        /*REQUEST*/
        [Display(Name = "Request ID")]
        public int REQ_ID { get; set; }

        [Display(Name = "Date Submitted")]
        public string REQUESTDATE { get; set; }

        /*REQUESTDETAILS*/
        [Display(Name = "Request Details ID")]
        public int REQUESTDETAILS_ID { get; set; }

        [Display(Name = "Security Class")]
        public string SCLASSDESC { get; set; }

        [Display(Name = "Reason for Access")]
        public string REASON_FOR_ACCESS { get; set; }

        /*ACCESSREQTYPE*/
        [Display(Name = "Request Type ID")]
        public int REQTYPE_ID { get; set; }

        [Display(Name = "Request Type")]
        public string REQTYPE_DESC { get; set; }

        /*REQUESTAPPROVALS*/
        [Display(Name = "Approval ID")]
        public int APPROVAL_ID { get; set; }

        [Display(Name = "Approver Rocket ID")]
        public string APPROVAL_ROCKETID { get; set; }

        [Display(Name = "Last Update")]
        public DateTime ACTIONDATE { get; set; }

        [Display(Name = "Signed?")]
        public int USERACCEPTANCESIGN { get; set; }

        [Display(Name = "Reason of Denial")]
        public string REASON_OF_DENIAL { get; set; }

        [Display(Name = "Request Status ID")]
        public int APPROVALSTATUS_ID { get; set; }

        /*REQUESTSTATUS*/
        [Display(Name = "Request Status ID")]
        public int REQSTATUS_ID { get; set; }

        [Display(Name = "Status")]
        public string REQUESTSTATUS_DESCRIPTION { get; set; }

        /*APPROVALLEVELS*/
        [Display(Name = "Approval Level ID")]
        public int AL_ID { get; set; }

        [Display(Name = "Awaiting Approval By")]
        public string AL_DESCRIPTION { get; set; }

        /*EMPLOYEES*/
        [Display(Name = "First Name")]
        public string FIRST_NAME { get; set; }

        [Display(Name = "Last Name")]
        public string LAST_NAME { get; set; }

    }
}