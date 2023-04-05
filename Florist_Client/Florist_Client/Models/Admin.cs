using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Admin
    {
        public int Customer_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
        public DateTime Birth_day { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}