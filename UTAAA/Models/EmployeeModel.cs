using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UTAAA.Models
{
    public class EmployeeModel
    {
        [Display(Name = "Rocket ID"), Required]
        public string ROCKET_ID { get; set; }

        [Display(Name="First Name"), Required]
        public string FIRST_NAME { get; set; }

        [Display(Name = "MI")]
        public char MIDDLE_INITIAL { get; set; }

        [Display(Name = "Last Name"), Required]
        public string LAST_NAME { get; set; }
        
        [Display(Name = "Job Title"), Required]
        public string TITLE { get; set; }
        
        [Display(Name = "Phone Number"), Required]
        public string PHONE_NUMBER { get; set; }
        
        [Display(Name = "Email"), Required]
        public string EMAIL { get; set; }
        
        [Display(Name = "Supervisor"), Required]
        public string SUPERVISOR_NAME { get; set; }
        
        [Display(Name = "Supervisor's Rocket ID"), Required]
        public string SUPERVISOR_ROCKETID { get; set; }
        
        [Display(Name = "Supervisor's Email"), Required]
        public string SUPERVISOR_EMAIL { get; set; }
    }
}