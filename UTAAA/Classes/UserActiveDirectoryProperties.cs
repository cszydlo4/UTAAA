using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace ConsoleAppSearchADTest
{
    public class UserActiveDirectoryProperties
    {
        public IEnumerable GetUserActiveDirectoryProperties(string term)
        {
           
            var entry = new DirectoryEntry(ConfigurationManager.AppSettings["ADPath"]);
            var filter = $"(&(objectClass=user)(sAMAccountName={term}*))";
            var properties = new[] { "samaccountname", "displayname", "employeeid", "sn", "givenname", "mail", "organization" };

            var searcher = new DirectorySearcher(entry, filter, properties) { SizeLimit = 100 };
            var results = searcher.FindAll();

            return GetEntries(results);
        }


        public IEnumerable GetEntries(SearchResultCollection result)
        {
            foreach (SearchResult entry in result)
            {
                if (entry.Properties["samaccountname"].Count > 0 &&
                    entry.Properties["displayname"].Count > 0 &&
                    entry.Properties["employeeid"].Count > 0 &&
                    entry.Properties["sn"].Count > 0 &&
                    entry.Properties["givenname"].Count > 0 &&
                    entry.Properties["mail"].Count > 0)
                {
                    yield return new
                    {
                        Username = entry.Properties["samaccountname"][0].ToString(),
                        DisplayName = entry.Properties["displayname"][0].ToString(),
                        RocketId = entry.Properties["employeeid"][0].ToString(),
                        Sn = entry.Properties["sn"][0].ToString(),
                        GivenName = entry.Properties["givenname"][0].ToString(),
                        EmailAddress = entry.Properties["mail"][0].ToString()
                    };
                }
            }
        }

        public UserActiveDirectoryProperties()
        {
            
        }
    }
}
