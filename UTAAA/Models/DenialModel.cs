using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class DenialModel
    {
        public int REQUESTDETAILS_ID { get; set; }
        public int REQSTATUS_ID { get; set; } = 3;
        [Display(Name = "Reason for Denial"), DataType(DataType.MultilineText), Required]
        public string REASON_OF_DENIAL { get; set; }
    }
}