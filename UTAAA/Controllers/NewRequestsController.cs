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
    public class NewRequestsController : Controller
    {
        /* GET: Index contains employee form */
        public ActionResult Index()
        {
            return PartialView();
        }

        /* Save employee object to database */
        [HttpPost]
        public ActionResult Index(EmployeeModel employee)
        {
            List<string> employeeIDs = new List<string>();

            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                if (ModelState.IsValid)
                {
                    employeeIDs = dbConn.Query<string>(@"SELECT ROCKET_ID FROM EMPLOYEES").ToList();
                    if (!employeeIDs.Contains(employee.ROCKET_ID)) // if entered rocket ID is not already saved in the database, insert record
                    {
                        string sqlQuery = "INSERT INTO EMPLOYEES (ROCKET_ID, " +
                                                                 "FIRST_NAME, " +
                                                                 "MIDDLE_INITIAL, " +
                                                                 "LAST_NAME, " +
                                                                 "TITLE, " +
                                                                 "PHONE_NUMBER, " +
                                                                 "EMAIL, " +
                                                                 "SUPERVISOR_NAME, " +
                                                                 "SUPERVISOR_ROCKETID, " +
                                                                 "SUPERVISOR_EMAIL) " +
                                                                 "Values('" + employee.ROCKET_ID + "', '" +
                                                                 employee.FIRST_NAME + "', '" +
                                                                 employee.MIDDLE_INITIAL + "', '" +
                                                                 employee.LAST_NAME + "', '" +
                                                                 employee.TITLE + "', '" +
                                                                 employee.PHONE_NUMBER + "', '" +
                                                                 employee.EMAIL + "', '" +
                                                                 employee.SUPERVISOR_NAME + "', '" +
                                                                 employee.SUPERVISOR_ROCKETID + "', '" +
                                                                 employee.SUPERVISOR_EMAIL + "')";

                        int rowsAffected = dbConn.Execute(sqlQuery);
                        return PartialView("Access"); // Move to access request form
                    }
                    else
                    {
                        return PartialView("EmployeeExists"); // Temporary for testing
                    }
                } else
                {
                    return PartialView("Index");
                }
                
            }
        }
    }
}