//Makes db connection string simpler

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public static class HelperModel
    {
        public static string cnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}