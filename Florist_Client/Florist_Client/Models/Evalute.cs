using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Evalute
    {
        public int Evalute_id { get; set; }
        public int Create_by { get; set; }
        public string Product_id { get; set; }
        public string Evalute_content { get; set; }
        public int Rate { get; set; }
        public DateTime Create_at { get; set; }
        public string Customer_Name { get; set; }
        public string Product_name { get; set; }
    }
}