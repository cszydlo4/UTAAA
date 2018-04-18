using Dapper;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTAAA.Models;

namespace UTAAA.Controllers
{
    public class PastRequestsController : Controller
    {
        string testRocketID = "R25419782";

        // GET: PastRequests
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();

            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                requests = dbConn.Query<RequestModel>(@"SELECT REQUESTAPPROVALS.ACTIONDATE, SECURITYCLASS.SCLASSDESC, ACCESSREQTYPE.REQTYPE_DESC, 
                                                            REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION 
                                                        FROM REQUESTDETAILS 
                                                        INNER JOIN SECURITYCLASS ON REQUESTDETAILS.SECURITYCLASS_ID = SECURITYCLASS.SECURITYCLASS_ID 
                                                        INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID 
                                                        INNER JOIN ACCESSREQTYPE ON REQUESTDETAILS.REQTYPE_ID = ACCESSREQTYPE.REQTYPE_ID 
                                                        INNER JOIN REQUESTSTATUS ON REQUESTDETAILS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID 
                                                        INNER JOIN REQUEST ON REQUEST.REQ_ID = REQUESTDETAILS.REQ_ID 
                                                        WHERE REQUEST.ROCKET_ID = '" + testRocketID + @"' 
                                                        ORDER BY REQUESTAPPROVALS.ACTIONDATE").ToList();
            }

            return PartialView(requests);
        }
    }
}