using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class ProductCart
    {
        public string id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public string img { get; set; }
        public float discount { get; set; }
    }
}