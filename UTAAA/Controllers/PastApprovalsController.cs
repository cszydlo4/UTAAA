﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UTAAA.Controllers
{
    public class PastApprovalsController : Controller
    {
        // GET: PastApprovals
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}