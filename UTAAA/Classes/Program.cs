using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppSearchADTest;
using System.Configuration;

namespace ConsoleAppSearchADTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UserActiveDirectoryProperties uadp = new UserActiveDirectoryProperties();
            var adProperties = uadp.GetUserActiveDirectoryProperties("UtadIDGoesHere");

            string Username = String.Empty;
            string DisplayName = String.Empty;
            string RocketId = String.Empty;
            string Sn = String.Empty;
            string GivenName = String.Empty;
            string EmailAddress = String.Empty;            

            foreach (var item in adProperties)
            {
                Username = item.GetType().GetProperty("Username").GetValue(item, null).ToString();
                DisplayName = item.GetType().GetProperty("DisplayName").GetValue(item, null).ToString();
                RocketId = item.GetType().GetProperty("RocketId").GetValue(item, null).ToString();
                Sn = item.GetType().GetProperty("Sn").GetValue(item, null).ToString();
                GivenName = item.GetType().GetProperty("GivenName").GetValue(item, null).ToString();
                EmailAddress = item.GetType().GetProperty("EmailAddress").GetValue(item, null).ToString();

                Console.WriteLine(Username);
                Console.WriteLine(DisplayName);
                Console.WriteLine(RocketId);
                Console.WriteLine(Sn);
                Console.WriteLine(GivenName);
                Console.WriteLine(EmailAddress);
            }

            //Email email = new Email();
            //email.SendEmail(EmailAddress, "This is a test.", "This is a test of the email system.", true, ConfigurationManager.AppSettings["emailServer"].ToString());
                        
            Console.ReadLine();
        }
    }
}
