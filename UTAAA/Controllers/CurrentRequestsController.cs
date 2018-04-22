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

        /*-----------------------------Index View--------------------------------*/
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();

            //requests.Add(new RequestModel { RequestID = 1, RequestDate = "4/6/2018", RequestStatus = 1, ApprovalLevel = 1 });
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                requests = dbConn.Query<RequestModel>(@"SELECT REQUEST.REQUESTDATE, SECURITYCLASS.SCLASSDESC, ACCESSREQTYPE.REQTYPE_DESC, APPROVALLEVELS.AL_DESCRIPTION, 
                                                            REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION, REQUESTDETAILS.REQUESTDETAILS_ID, REQUESTAPPROVALS.REQSTATUS_ID
                                                        FROM REQUESTDETAILS 
                                                        INNER JOIN SECURITYCLASS ON REQUESTDETAILS.SECURITYCLASS_ID = SECURITYCLASS.SECURITYCLASS_ID 
                                                        INNER JOIN REQUESTAPPROVALS ON REQUESTDETAILS.REQUESTDETAILS_ID = REQUESTAPPROVALS.REQUESTDETAILS_ID 
                                                        INNER JOIN ACCESSREQTYPE ON REQUESTDETAILS.REQTYPE_ID = ACCESSREQTYPE.REQTYPE_ID 
                                                        INNER JOIN APPROVALLEVELS ON REQUESTAPPROVALS.AL_ID = APPROVALLEVELS.AL_ID 
                                                        INNER JOIN REQUESTSTATUS ON REQUESTAPPROVALS.REQSTATUS_ID = REQUESTSTATUS.REQSTATUS_ID 
                                                        INNER JOIN REQUEST ON REQUEST.REQ_ID = REQUESTDETAILS.REQ_ID 
                                                        WHERE REQUEST.ROCKET_ID = '" + testRocketID + @"' 
                                                        ORDER BY REQUEST.REQUESTDATE").ToList();
            }

                return PartialView(requests);
        }

        /*-----------------------------Details View--------------------------------*/
        public ActionResult Details(int REQUESTDETAILS_ID)
        {
            List<AccessModel> accessRequest = new List<AccessModel>();
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                accessRequest = dbConn.Query<AccessModel>(@"SELECT SECURITYCLASS.SCLASSDESC, SUBJECT_AREAS.SUBJECTAREANAME, SECURITY_ACCESS.SECURITYACCESSDESC, 
                                                                SYSTEMS.SYSTEMNAME, REQUESTDETAILS.REASON_FOR_ACCESS, REQUESTAPPROVALS.ACTIONDATE, 
                                                                REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION, APPROVALLEVELS.AL_DESCRIPTION, ACCESSREQTYPE.REQTYPE_DESC,
                                                                REQUEST.REQUESTDATE
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
                                                            WHERE REQUESTDETAILS.REQUESTDETAILS_ID = " + REQUESTDETAILS_ID).ToList();
            }

            foreach (var item in accessRequest)
            {
                if (item.REQUESTSTATUS_DESCRIPTION == "Pending")
                {
                    return PartialView(item); // Only returns the final approval in the chain
                }
            }
            return PartialView(); // Will not run
        }
    }
}