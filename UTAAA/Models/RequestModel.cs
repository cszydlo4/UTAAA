using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class RequestModel
    {
        
        public int REQ_ID { get; set; }
        public string REQUESTDATE { get; set; }
        public string AL_DESCRIPTION { get; set; }
        public string REQUESTSTATUS_DESCRIPTION { get; set; }
        public DateTime ACTIONDATE { get; set; }
    }
}