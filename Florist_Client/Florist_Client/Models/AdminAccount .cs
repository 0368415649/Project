using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class AdminAccount
    {
        public int Account_id { get; set; }
        [Required]
        public string User_name { get; set; }
        [Required]
        public string Password { get; set; }
        public int Group_id { get; set; }
        public string Group_name { get; set; }
        [Required]
        public string Employee_name { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}