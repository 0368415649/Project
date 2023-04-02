using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class ChangePass
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string ComfirmPassword { get; set; }
        
    }
}