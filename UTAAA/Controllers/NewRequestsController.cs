using Dapper;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTAAA.Models;
using ConsoleAppSearchADTest;
using System.Configuration;

namespace UTAAA.Controllers
{
    public class NewRequestsController : Controller
    {
        /*--------------------------Index View (Employeee form)-----------------------------*/
        public ActionResult Index()
        {
            UserActiveDirectoryProperties ad = new UserActiveDirectoryProperties();
            var dscheel = ad.GetUserActiveDirectoryProperties("dscheel");

            Email em = new Email();
            em.SendEmail("dante.scheele@rockets.utoledo.edu", "TEST", "Body", ConfigurationManager.AppSettings["emailServer"].ToString());

            return PartialView();
        }

        /*-------------------------Index View (Push employee to db)-------------------------*/
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
                        //return PartialView("Access"); // Move to access request form
                    }

                    List<AccessModel> systems = new List<AccessModel>();

                    systems = dbConn.Query<AccessModel>(@"SELECT SYSTEMNAME FROM SYSTEMS").ToList();
                    foreach (var system in systems)
                    {
                        system.ROCKET_ID = employee.ROCKET_ID;
                    }

                    return PartialView("Systems", systems);
                } else
                {
                    return PartialView("Index");
                }
            }
        }

        /*----------------------------------Subject Areas View---------------------------------*/
        public ActionResult SubjectAreas(string SYSTEMNAME, string ROCKET_ID)
        {
            List<AccessModel> subjectAreas = new List<AccessModel>();

            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                List<int> SYSTEMS_ID = dbConn.Query<int>(@"SELECT SYSTEMS_ID FROM SYSTEMS WHERE SYSTEMNAME = '" + SYSTEMNAME + "'").ToList();
                subjectAreas = dbConn.Query<AccessModel>(@"SELECT SUBJECTAREANAME FROM SUBJECT_AREAS INNER JOIN SECURITYCLASS ON SUBJECT_AREAS.SUBJECTAREA_ID = SECURITYCLASS.SUBJECTAREA_ID WHERE SYSTEMS_ID = " + SYSTEMS_ID[0] + " ORDER BY SUBJECTAREANAME").ToList();
                subjectAreas = subjectAreas.GroupBy(x => x.SUBJECTAREANAME).Select(x => x.First()).ToList(); //Remove duplicates

                foreach (var subjectArea in subjectAreas)
                {
                    subjectArea.ROCKET_ID = ROCKET_ID;
                    subjectArea.SYSTEMNAME = SYSTEMNAME;
                }

                return PartialView("SubjectAreas", subjectAreas);
            }
        }

        /*----------------------------------Security Access View---------------------------------*/
        public ActionResult SecurityAccess(string SUBJECTAREANAME, string ROCKET_ID, string SYSTEMNAME)
        {
            List<AccessModel> securityAccess = new List<AccessModel>();

            using (OracleConnection dbConn = new OracleConnection(HelperModel.cnnVal("OracleDB")))
            {
                List<int> SUBJECTAREA_ID = dbConn.Query<int>(@"SELECT SUBJECTAREA_ID FROM SUBJECT_AREAS WHERE SUBJECTAREANAME = '" + SUBJECTAREANAME + "'").ToList();
                List<int> SYSTEMS_ID = dbConn.Query<int>(@"SELECT SYSTEMS_ID FROM SYSTEMS WHERE SYSTEMNAME = '" + SYSTEMNAME + "'").ToList();
                securityAccess = dbConn.Query<AccessModel>(@"SELECT SECURITYACCESSDESC FROM SECURITY_ACCESS INNER JOIN SECURITYCLASS ON SECURITY_ACCESS.SECURITYACCESS_ID = SECURITYCLASS.SECURITYACCESS_ID WHERE SUBJECTAREA_ID = " + SUBJECTAREA_ID[0] + "AND SYSTEMS_ID = " + SYSTEMS_ID[0] + " ORDER BY SECURITYACCESSDESC").ToList();
                securityAccess = securityAccess.GroupBy(x => x.SECURITYACCESSDESC).Select(x => x.First()).ToList(); //Remove duplicates

                foreach (var access in securityAccess)
                {
                    access.ROCKET_ID = ROCKET_ID;
                }

                return PartialView("SecurityAccess", securityAccess);
            }
        }
    }
}