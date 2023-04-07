using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class OrderDetails
    {
        public int Order_detail_id { get; set; }
        public string Product_id { get; set; }
        public string Image { get; set; }
        public string Product_Name { get; set; }
        public int Order_id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
    }
}