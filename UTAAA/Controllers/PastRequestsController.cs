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

        /*-----------------------------Index View----------------------------------*/
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                requests = dbConn.Query<RequestModel>(@"SELECT REQUESTAPPROVALS.ACTIONDATE, SECURITYCLASS.SCLASSDESC, ACCESSREQTYPE.REQTYPE_DESC, 
                                                            REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION, REQUESTDETAILS.REQUESTDETAILS_ID
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

        /*-----------------------------Details View--------------------------------*/
        public ActionResult Details(int REQUESTDETAILS_ID)
        {
            List<AccessModel> accessRequest = new List<AccessModel>();
            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                accessRequest = dbConn.Query<AccessModel>(@"SELECT SECURITYCLASS.SCLASSDESC, SUBJECT_AREAS.SUBJECTAREANAME, SECURITY_ACCESS.SECURITYACCESSDESC, 
                                                                SYSTEMS.SYSTEMNAME, REQUESTDETAILS.REASON_FOR_ACCESS, REQUESTAPPROVALS.ACTIONDATE, 
                                                                REQUESTAPPROVALS.REASON_OF_DENIAL, REQUESTSTATUS.REQUESTSTATUS_DESCRIPTION, 
                                                                APPROVALLEVELS.AL_DESCRIPTION, ACCESSREQTYPE.REQTYPE_DESC, REQUEST.REQUESTDATE
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
                                                            WHERE REQUESTDETAILS.REQUESTDETAILS_ID = " + REQUESTDETAILS_ID).ToList();
            }
            
            if (accessRequest[0].REASON_OF_DENIAL == null)
            {
                accessRequest[0].REASON_OF_DENIAL = "N/A";
            }

            return PartialView(accessRequest[0]);
        }
    }
}