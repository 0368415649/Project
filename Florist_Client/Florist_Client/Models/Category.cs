using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Category
    {
        public int Category_id { get; set; }
        [Required]
        public string Category_name { get; set; }
    }
}