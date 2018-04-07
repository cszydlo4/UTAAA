using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UTAAA.Controllers
{
    public class CurrentApprovalsController : Controller
    {
        // GET: CurrentApprovals
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}