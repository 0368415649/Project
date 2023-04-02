using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class Check_OutController : Controller
    {
        // GET: Check_Out
        public ActionResult Index(int? id)
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            int cusID = 0;
            if (Request.Cookies["customerID"] != null)
            {
                cusID = int.Parse(Request.Cookies["customerID"].Value);
            }
            dataSet = webService.getAddressByCustomer(cusID);
            List<Flower_recipient> listFlowerRecipient = new List<Flower_recipient>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Flower_recipient flower_Recipient = new Flower_recipient();
                flower_Recipient.Flower_recipient_id = int.Parse(item["Flower_recipient_id"].ToString());
                flower_Recipient.Name = item["Name"].ToString();
                flower_Recipient.Address = item["Address"].ToString();
                flower_Recipient.Phone = item["Phone"].ToString();
                flower_Recipient.Customer_id = int.Parse(item["Customer_id"].ToString());
                listFlowerRecipient.Add(flower_Recipient);
            }
            Flower_recipient flower_Recipient_buy = new Flower_recipient();
            if (id != null)
            {
                dataSet = webService.getAddressByID((int)id);
                flower_Recipient_buy.Flower_recipient_id = int.Parse(dataSet.Tables[0].Rows[0]["Flower_recipient_id"].ToString());
                flower_Recipient_buy.Name = dataSet.Tables[0].Rows[0]["Name"].ToString();
                flower_Recipient_buy.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
                flower_Recipient_buy.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
                flower_Recipient_buy.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
            }
            ViewBag.listFlowerRecipient = listFlowerRecipient;
            

            return View(flower_Recipient_buy);
        }

        [HttpPost]
        public ActionResult Index(int? id ,Flower_recipient flower_Recipient)
        {
            if (ModelState.IsValid)
            {
                Session["flower_Recipient"] = flower_Recipient;
                return RedirectToAction("index", "CormfirmCalendar");
            }
            else
            {
                api_product.WebService webService = new api_product.WebService();
                DataSet dataSet = new DataSet();
                int cusID = 0;
                if (Request.Cookies["customerID"] != null)
                {
                    cusID = int.Parse(Request.Cookies["customerID"].Value);
                }
                dataSet = webService.getAddressByCustomer(cusID);
                List<Flower_recipient> listFlowerRecipient = new List<Flower_recipient>();
                foreach (DataRow item in dataSet.Tables[0].Rows)
                {
                    Flower_recipient flower_Recipient2 = new Flower_recipient();
                    flower_Recipient2.Flower_recipient_id = int.Parse(item["Flower_recipient_id"].ToString());
                    flower_Recipient2.Name = item["Name"].ToString();
                    flower_Recipient2.Address = item["Address"].ToString();
                    flower_Recipient2.Phone = item["Phone"].ToString();
                    flower_Recipient2.Customer_id = int.Parse(item["Customer_id"].ToString());
                    listFlowerRecipient.Add(flower_Recipient2);
                }
                Flower_recipient flower_Recipient_buy = new Flower_recipient();
                if (id != null)
                {
                    dataSet = webService.getAddressByID((int)id);
                    flower_Recipient_buy.Flower_recipient_id = int.Parse(dataSet.Tables[0].Rows[0]["Flower_recipient_id"].ToString());
                    flower_Recipient_buy.Name = dataSet.Tables[0].Rows[0]["Name"].ToString();
                    flower_Recipient_buy.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
                    flower_Recipient_buy.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
                    flower_Recipient_buy.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
                }
                ViewBag.listFlowerRecipient = listFlowerRecipient;
                return View(flower_Recipient_buy);
            }

        }
    }
}