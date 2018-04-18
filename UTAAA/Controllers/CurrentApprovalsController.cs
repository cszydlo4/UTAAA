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

        // GET: CurrentApprovals
        public ActionResult Index()
        {
            List<RequestModel> approvals = new List<RequestModel>();
            
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                approvals = dbConn.Query<RequestModel>(@"SELECT REQUEST.REQUESTDATE, SECURITYCLASS.SCLASSDESC, ACCESSREQTYPE.REQTYPE_DESC, EMPLOYEES.FIRST_NAME, 
                                                                EMPLOYEES.LAST_NAME, REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION 
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
    }
}