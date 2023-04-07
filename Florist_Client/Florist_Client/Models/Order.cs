using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Order
    {
        public int Order_id { get; set; }
        public string Status { get; set; }
        public int Shipper_id { get; set; }
        public string ShipperName { get; set; }
        public int Flower_recipient_id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime Create_at { get; set; }
        public int Create_by { get; set; }
        public string Create_Name { get; set; }
        public int Message_id { get; set; }
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Received_date { get; set; }
        public string Received_time { get; set; }
        public float Total { get; set; }
        public List<OrderDetails> orderDetails { get; set; }
    }
}