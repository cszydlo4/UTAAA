using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTAAA.Models;
using Dapper;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Configuration;

namespace UTAAA.Controllers
{
    public class CurrentApprovalsController : Controller
    {
        string testRocketID = "R25419782";

        /*-----------------------------Index View----------------------------------*/
        public ActionResult Index()
        {
            List<RequestModel> approvals = new List<RequestModel>();
            
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                approvals = dbConn.Query<RequestModel>(@"SELECT REQUEST.REQUESTDATE, SECURITYCLASS.SCLASSDESC, ACCESSREQTYPE.REQTYPE_DESC, EMPLOYEES.FIRST_NAME, 
                                                                EMPLOYEES.LAST_NAME, REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION, REQUESTDETAILS.REQUESTDETAILS_ID 
                                                         FROM EMPLOYEES 
                                                         INNER JOIN REQUEST ON EMPLOYEES.ROCKET_ID = REQUEST.ROCKET_ID 
                                                         INNER JOIN REQUESTDETAILS ON REQUESTDETAILS.REQ_ID = REQUEST.REQ_ID 
                                                         INNER JOIN SECURITYCLASS ON REQUESTDETAILS.SECURITYCLASS_ID = SECURITYCLASS.SECURITYCLASS_ID 
                                                         INNER JOIN ACCESSREQTYPE ON REQUESTDETAILS.REQTYPE_ID = ACCESSREQTYPE.REQTYPE_ID 
                                                         INNER JOIN REQUESTSTATUS ON REQUESTDETAILS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID 
                                                         INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID 
                                                         WHERE REQUESTAPPROVALS.APPROVAL_ROCKETID = '" + testRocketID + @"' 
                                                         ORDER BY REQUEST.REQUESTDATE").ToList();
            }

            return PartialView(approvals);
        }

        /*-----------------------------Details View--------------------------------*/
        public ActionResult Details(int REQUESTDETAILS_ID)
        {
            List<AccessModel> accessRequest = new List<AccessModel>();
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                accessRequest = dbConn.Query<AccessModel>(@"SELECT SECURITYCLASS.SCLASSDESC, SUBJECT_AREAS.SUBJECTAREANAME, SECURITY_ACCESS.SECURITYACCESSDESC, 
                                                                SYSTEMS.SYSTEMNAME, REQUESTDETAILS.REASON_FOR_ACCESS, REQUESTAPPROVALS.ACTIONDATE, 
                                                                REQUESTAPPROVALS.REASON_OF_DENIAL, REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION, 
                                                                APPROVALLEVELS.AL_DESCRIPTION, ACCESSREQTYPE.REQTYPE_DESC, REQUEST.REQUESTDATE,
                                                                EMPLOYEES.FIRST_NAME, EMPLOYEES.LAST_NAME, EMPLOYEES.EMAIL, EMPLOYEES.TITLE,
                                                                EMPLOYEES.PHONE_NUMBER, EMPLOYEESTATUS.STATUS_DESC, DEPARTMENT.DEPT_NAME, EMPLOYEES.SUPERVISOR_NAME,
                                                                EMPLOYEES.SUPERVISOR_EMAIL
                                                            FROM SECURITYCLASS 
                                                            INNER JOIN SUBJECT_AREAS ON SECURITYCLASS.SUBJECTAREA_ID = SUBJECT_AREAS.SUBJECTAREA_ID 
                                                            INNER JOIN SECURITY_ACCESS ON SECURITYCLASS.SECURITYACCESS_ID = SECURITY_ACCESS.SECURITYACCESS_ID 
                                                            INNER JOIN SYSTEMS ON SECURITYCLASS.SYSTEMS_ID = SYSTEMS.SYSTEMS_ID 
                                                            INNER JOIN REQUESTDETAILS ON SECURITYCLASS.SECURITYCLASS_ID = REQUESTDETAILS.SECURITYCLASS_ID 
                                                            INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID
                                                            INNER JOIN REQUESTSTATUS ON REQUESTDETAILS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID
                                                            INNER JOIN APPROVALLEVELS ON REQUESTAPPROVALS.AL_ID = APPROVALLEVELS.AL_ID
                                                            INNER JOIN ACCESSREQTYPE ON REQUESTDETAILS.REQTYPE_ID = ACCESSREQTYPE.REQTYPE_ID
                                                            INNER JOIN REQUEST ON REQUESTDETAILS.REQ_ID = REQUEST.REQ_ID
                                                            INNER JOIN EMPLOYEES ON REQUEST.ROCKET_ID = EMPLOYEES.ROCKET_ID
                                                            INNER JOIN EMPLOYEESTATUS ON EMPLOYEES.STATUS_ID = EMPLOYEESTATUS.STATUS_ID
                                                            INNER JOIN DEPARTMENT ON EMPLOYEES.DEPT_ID = DEPARTMENT.DEPT_ID
                                                            WHERE REQUESTDETAILS.REQUESTDETAILS_ID = " + REQUESTDETAILS_ID).ToList();
            }
            
            return PartialView(accessRequest[0]);
        }
    }
}