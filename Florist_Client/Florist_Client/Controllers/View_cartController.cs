using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Florist_Client.Controllers
{
    public class View_cartController : Controller
    {
        // GET: View_cart
        public ActionResult Index()
        {
            if (Request.Cookies["customerID"] != null)
            {
                api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            int cusID = 0;
            
                cusID = int.Parse(Request.Cookies["customerID"].Value);
           
            dataSet = webService.GetAllMessage(cusID);
            List<Messages> listMessage = new List<Messages>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Messages message = new Messages();
                message.Message_id = int.Parse(item["Message_id"].ToString());
                message.Message_content = item["Message_content"].ToString();
                message.Category = item["Category"].ToString();
                message.Customer_id = (item["Customer_id"].ToString() != "") ? int.Parse(item["Customer_id"].ToString()) : 0;
                listMessage.Add(message);
            }
            ViewBag.listMessage = listMessage;
            return View();
            }
            else
            {
                Session["notLogin"] = "true";
                return RedirectToAction("index", "homepage");

            }
        }
    }
}