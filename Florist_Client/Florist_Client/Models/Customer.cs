using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Customer
    {
        public int Customer_id { get; set; }

        [Required]
        [StringLength(20)]
        public string First_name { get; set; }
        [Required]
        [StringLength(20)]
        public string  Last_name { get; set; }
        public string Password { get; set; }
        [Required]
        public string Sex { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Birth_day { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string Phone  { get; set; }
        public string Address { get; set; }
    } 
}