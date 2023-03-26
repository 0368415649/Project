using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Products
    {
        public string Product_id { get; set; }
        public string Product_name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public int Category_id { get; set; }
        public string Category_name { get; set; }
        public int Supplier_id { get; set; }
        public string Supplier_name { get; set; }
        public int Update_by { get; set; }
        public string User_name { get; set; }
        public int Sold { get; set; }
        public int Inventory { get; set; }
        public DateTime Update_at { get; set; }
    }
}