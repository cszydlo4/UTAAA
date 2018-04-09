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
        int testRocketID = 12345678;

        // GET: PastRequests
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();

            //requests.Add(new RequestModel { RequestID = 1, RequestDate = "4/6/2018", RequestStatus = 1, ApprovalLevel = 1 });
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                //requests = dbConn.Query<RequestModel>("SELECT REQ_ID, REQUESTDATE, REQUESTSTATUS_DESCRIPTION FROM REQUEST WHERE ROCKET_ID = " + testRocketID).ToList();
                requests = dbConn.Query<RequestModel>("SELECT REQUEST.REQ_ID, REQUEST.REQUESTDATE, REQUESTAPPROVALS.ACTIONDATE, REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION FROM REQUEST INNER JOIN REQUESTAPPROVALS ON REQUEST.REQ_ID=REQUESTAPPROVALS.REQ_ID INNER JOIN REQUESTSTATUS ON REQUESTAPPROVALS.REQSTATUSID = REQUESTSTATUS.REQSTATUS_ID WHERE REQUEST.ROCKET_ID = " + testRocketID + " ORDER BY REQUEST.REQ_ID").ToList();

            }

            return PartialView(requests);
        }
    }
}