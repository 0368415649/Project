using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Supplier
    {
        public int Supplier_id { get; set; }
        [Required]
        public string Supplier_name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }

    }
}