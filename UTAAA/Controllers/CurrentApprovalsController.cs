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
                                                         INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID 
                                                         INNER JOIN REQUESTSTATUS ON REQUESTAPPROVALS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID 
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
                                                                EMPLOYEES.SUPERVISOR_EMAIL, REQUESTAPPROVALS.REQSTATUS_ID, REQUESTDETAILS.REQUESTDETAILS_ID,
                                                                REQUESTAPPROVALS.APPROVAL_ROCKETID
                                                            FROM SECURITYCLASS 
                                                            INNER JOIN SUBJECT_AREAS ON SECURITYCLASS.SUBJECTAREA_ID = SUBJECT_AREAS.SUBJECTAREA_ID 
                                                            INNER JOIN SECURITY_ACCESS ON SECURITYCLASS.SECURITYACCESS_ID = SECURITY_ACCESS.SECURITYACCESS_ID 
                                                            INNER JOIN SYSTEMS ON SECURITYCLASS.SYSTEMS_ID = SYSTEMS.SYSTEMS_ID 
                                                            INNER JOIN REQUESTDETAILS ON SECURITYCLASS.SECURITYCLASS_ID = REQUESTDETAILS.SECURITYCLASS_ID 
                                                            INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID
                                                            INNER JOIN REQUESTSTATUS ON REQUESTAPPROVALS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID
                                                            INNER JOIN APPROVALLEVELS ON REQUESTAPPROVALS.AL_ID = APPROVALLEVELS.AL_ID
                                                            INNER JOIN ACCESSREQTYPE ON REQUESTDETAILS.REQTYPE_ID = ACCESSREQTYPE.REQTYPE_ID
                                                            INNER JOIN REQUEST ON REQUESTDETAILS.REQ_ID = REQUEST.REQ_ID
                                                            INNER JOIN EMPLOYEES ON REQUEST.ROCKET_ID = EMPLOYEES.ROCKET_ID
                                                            INNER JOIN EMPLOYEESTATUS ON EMPLOYEES.STATUS_ID = EMPLOYEESTATUS.STATUS_ID
                                                            INNER JOIN DEPARTMENT ON EMPLOYEES.DEPT_ID = DEPARTMENT.DEPT_ID
                                                            WHERE REQUESTDETAILS.REQUESTDETAILS_ID = " + REQUESTDETAILS_ID).ToList();
            }

            foreach (var item in accessRequest)
            {
                if (item.APPROVAL_ROCKETID == testRocketID)
                {
                    return PartialView(item); // Only returns the final approval in the chain
                }
            }
            return PartialView(); // Will not run
        }

        /*-----------------------------Deny View--------------------------------*/
        public ActionResult Deny(int REQUESTDETAILS_ID)
        {
            ApprovalModel denial = new ApprovalModel();
            denial.REQUESTDETAILS_ID = REQUESTDETAILS_ID;
            
            return PartialView(denial);
        }

        [HttpPost]
        public ActionResult Deny(ApprovalModel denial)
        {
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                if (ModelState.IsValid)
                {
                    // Mark request as denied
                    string sqlQuery1 = @"UPDATE REQUESTDETAILS SET REQSTATUS_ID = 3 WHERE REQUESTDETAILS_ID = " + denial.REQUESTDETAILS_ID;
                    // Mark approval as denied with reason of denial
                    string sqlQuery2 = @"UPDATE REQUESTAPPROVALS SET ACTIONDATE = SYSDATE, REQSTATUS_ID = 3, REASON_OF_DENIAL = '" + denial.REASON_OF_DENIAL + @"' WHERE REQUESTDETAILS_ID = " + denial.REQUESTDETAILS_ID + @" AND APPROVAL_ROCKETID = '" + testRocketID + "'";
                    
                    dbConn.Execute(sqlQuery1);
                    dbConn.Execute(sqlQuery2);
                    
                    return PartialView("Denied");
                } else
                {
                    return PartialView("Deny");
                }
            }
            
        }

        /*-----------------------------Approve View--------------------------------*/
        public ActionResult Approve(int REQUESTDETAILS_ID)
        {
            ApprovalModel approval = new ApprovalModel();
            approval.REQUESTDETAILS_ID = REQUESTDETAILS_ID;

            return PartialView(approval);
        }

        [HttpPost]
        public ActionResult Approve(ApprovalModel approval)
        {
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                /* Set approval to approved */
                string sqlQuery1 = @"UPDATE REQUESTAPPROVALS SET ACTIONDATE = SYSDATE, REQSTATUS_ID = 2 WHERE REQUESTDETAILS_ID = " + approval.REQUESTDETAILS_ID + @" AND APPROVAL_ROCKETID = '" + testRocketID + "'";
                dbConn.Execute(sqlQuery1);

                /* Retrieve Approval Level, Custodian Rocket ID, and Admin Rocket ID for Request*/
                List<int> AL_ID = dbConn.Query<int>("SELECT AL_ID FROM REQUESTAPPROVALS WHERE REQUESTDETAILS_ID = " + approval.REQUESTDETAILS_ID + @" AND APPROVAL_ROCKETID = '" + testRocketID + "'").ToList();
                List<string> PRIMARY_CUSTODIAN_ROCKETID = dbConn.Query<string>("SELECT PRIMARY_CUSTODIAN_ROCKETID FROM SECURITYCLASS INNER JOIN REQUESTDETAILS ON SECURITYCLASS.SECURITYCLASS_ID = REQUESTDETAILS.SECURITYCLASS_ID WHERE REQUESTDETAILS.REQUESTDETAILS_ID = " + approval.REQUESTDETAILS_ID).ToList();
                List<string> ADMIN_ROCKETID = dbConn.Query<string>("SELECT ADMIN_ROCKETID FROM SECURITYCLASS INNER JOIN REQUESTDETAILS ON SECURITYCLASS.SECURITYCLASS_ID = REQUESTDETAILS.SECURITYCLASS_ID WHERE REQUESTDETAILS.REQUESTDETAILS_ID = " + approval.REQUESTDETAILS_ID).ToList();

                string sqlQuery2;
                if (AL_ID[0] == 2)
                {   //Elevate request to Custodian
                    sqlQuery2 = @"INSERT INTO REQUESTAPPROVALS (APPROVAL_ID, REQUESTDETAILS_ID, APPROVAL_ROCKETID, AL_ID, ACTIONDATE) VALUES (APPROVAL_ID.nextval, " + approval.REQUESTDETAILS_ID + @", '" + PRIMARY_CUSTODIAN_ROCKETID[0] + @"', 3, SYSDATE)";
                    dbConn.Execute(sqlQuery2);
                } else if (AL_ID[0] == 3)
                {
                    //Elevate request to Admin
                    sqlQuery2 = @"INSERT INTO REQUESTAPPROVALS (APPROVAL_ID, REQUESTDETAILS_ID, APPROVAL_ROCKETID, AL_ID, ACTIONDATE) VALUES (APPROVAL_ID.nextval, " + approval.REQUESTDETAILS_ID + @", '" + ADMIN_ROCKETID[0] + @"', 4, SYSDATE)";
                    dbConn.Execute(sqlQuery2);
                } else if (AL_ID[0] == 4)
                {
                    //Mark request as approved
                    sqlQuery2 = @"UPDATE REQUESTDETAILS SET REQSTATUS_ID = 2 WHERE REQUESTDETAILS_ID = " + approval.REQUESTDETAILS_ID;
                    dbConn.Execute(sqlQuery2);
                }

                return PartialView("Approved");
            }
        }
    }
}