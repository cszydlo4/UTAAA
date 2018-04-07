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
        // GET: CurrentRequests
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();

            //requests.Add(new RequestModel { RequestID = 1, RequestDate = "4/6/2018", RequestStatus = 1, ApprovalLevel = 1 });
            using (OracleConnection dbConn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString))
            {
               
                requests = dbConn.Query<RequestModel>("SELECT SECURITYACCESSDESC FROM SECURITY_ACCESS").ToList();
            }

                return PartialView(requests);
        }
    }
}