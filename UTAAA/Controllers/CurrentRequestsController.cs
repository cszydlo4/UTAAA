using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTAAA.Models;

namespace UTAAA.Controllers
{
    public class CurrentRequestsController : Controller
    {
        // GET: CurrentRequests
        public ActionResult Index()
        {
            List<RequestModel> requests = new List<RequestModel>();

            requests.Add(new RequestModel { RequestID = 1, RequestDate = "4/6/2018", RequestStatus = 1, ApprovalLevel = 1 });

            return PartialView(requests);
        }
    }
}