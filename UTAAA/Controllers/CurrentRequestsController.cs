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
    public class CurrentRequestsController : Controller
    {
        string testRocketID = "R25419782";

        // GET: CurrentRequests
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();

            //requests.Add(new RequestModel { RequestID = 1, RequestDate = "4/6/2018", RequestStatus = 1, ApprovalLevel = 1 });
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                requests = dbConn.Query<RequestModel>(@"SELECT REQUEST.REQUESTDATE, SECURITYCLASS.SCLASSDESC, ACCESSREQTYPE.REQTYPE_DESC, APPROVALLEVELS.AL_DESCRIPTION, 
                                                            REQUESTSTATUS_DESCRIPTION 
                                                        FROM REQUESTDETAILS 
                                                        INNER JOIN SECURITYCLASS ON REQUESTDETAILS.SECURITYCLASS_ID = SECURITYCLASS.SECURITYCLASS_ID 
                                                        INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID 
                                                        INNER JOIN ACCESSREQTYPE ON REQUESTDETAILS.REQTYPE_ID = ACCESSREQTYPE.REQTYPE_ID 
                                                        INNER JOIN APPROVALLEVELS ON REQUESTAPPROVALS.AL_ID = APPROVALLEVELS.AL_ID 
                                                        INNER JOIN REQUESTSTATUS ON REQUESTDETAILS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID 
                                                        INNER JOIN REQUEST ON REQUEST.REQ_ID = REQUESTDETAILS.REQ_ID 
                                                        WHERE REQUEST.ROCKET_ID = '" + testRocketID + @"' 
                                                        ORDER BY REQUEST.REQUESTDATE").ToList();
            }

                return PartialView(requests);
        }
    }
}