using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Image_Client.Models;

namespace Image_Client.Models
{
    public class Product
    {
       // string ProductID, string Product_name, string InputDescription, string Image, float Price, float Discount, 
         //   int Category_id, int Supplier_id, int Sold, int Inventory
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public int Sold { get; set; }
        public int Inventory { get; set; }
        public byte[] Picture { get; set; }

    }
}