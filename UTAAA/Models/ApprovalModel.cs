using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class ApprovalModel
    {
        public int REQUESTDETAILS_ID { get; set; }
        public int REQSTATUS_ID { get; set; }
        [Display(Name = "Reason for Denial"), DataType(DataType.MultilineText), Required]
        public string REASON_OF_DENIAL { get; set; }
        public int AL_ID { get; set; }
    }
}