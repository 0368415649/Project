using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Florist_Client.Models
{
    public class Messages
    {
        public int Message_id { get; set; }
        public string Message_content { get; set; }
        public string Category { get; set; }
        public int Customer_id { get; set; }

    }
}