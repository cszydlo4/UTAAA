using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class AccessModel
    {
        /*SECURITYCLASS*/
        [Display(Name = "Security Class")]
        public string SCLASSDESC { get; set; }

        /*SUBJECT_AREAS*/
        [Display(Name = "Subject Area")]
        public string SUBJECTAREANAME { get; set; }

        /*SECURITY_ACCESS*/
        [Display(Name = "Security Access")]
        public string SECURITYACCESSDESC { get; set; }

        /*SYSTEMS*/
        [Display(Name = "System")]
        public string SYSTEMNAME { get; set; }

        /*REQUEST*/
        [Display(Name = "Date Submitted")]
        public DateTime REQUESTDATE { get; set; }

        /*REQUESTDETAILS*/
        [Display(Name = "Reason for Access")]
        public string REASON_FOR_ACCESS { get; set; }

        /*REQUESTAPPROVALS*/
        [Display(Name = "Last Update")]
        public DateTime ACTIONDATE { get; set; }

        [Display(Name = "Reason for Denial")]
        public string REASON_OF_DENIAL { get; set; }

        /*REQUESTSTATUS*/
        [Display(Name = "Request Status")]
        public string REQUESTSTATUS_DESCRIPTION { get; set; }

        /*APPROVALLEVELS*/
        [Display(Name = "Level of Approval")]
        public string AL_DESCRIPTION { get; set; }

        /*ACCESSREQTYPE*/
        [Display(Name = "Request Type")]
        public string REQTYPE_DESC { get; set; }

        /*EMPLOYEES*/
        [Display(Name = "First Name")]
        public string FIRST_NAME { get; set; }

        [Display(Name = "Last Name")]
        public string LAST_NAME { get; set; }

        [Display(Name = "Email")]
        public string EMAIL { get; set; }

        [Display(Name = "Title")]
        public string TITLE { get; set; }

        [Display(Name = "Phone Number")]
        public string PHONE_NUMBER { get; set; }

        [Display(Name = "Supervisor"), Required]
        public string SUPERVISOR_NAME { get; set; }

        [Display(Name = "Supervisor's Email"), Required]
        public string SUPERVISOR_EMAIL { get; set; }

        /*EMPLOYEESTATUS*/
        [Display(Name = "Employee Status")]
        public string STATUS_DESC { get; set; }

        /*DEPARTMENT*/
        [Display(Name = "Department")]
        public string DEPT_NAME { get; set; }
    }
}