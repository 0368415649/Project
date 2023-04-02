using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class ProfileAddressController : Controller
    {
        // GET: ProfileAddress
        public ActionResult Index()
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
            ViewBag.profileAddress = "profileAddress";
            if(Session["bugDeleteAddress"] != null)
            {
                ViewBag.bugDeleteAddress = Session["bugDeleteAddress"];
                Session["bugDeleteAddress"] = null;
            }
            return View(listFlowerRecipient);
        }
        public ActionResult Create()
        {
            Flower_recipient flower_Recipient = new Flower_recipient();
            flower_Recipient.Customer_id = int.Parse(Request.Cookies["customerID"].Value);
            return View(flower_Recipient);
        }
        [HttpPost]
        public ActionResult Create(Flower_recipient flower_Recipient)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.insertAddressByCustomer(flower_Recipient.Customer_id, flower_Recipient.Name, flower_Recipient.Address, flower_Recipient.Phone);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAddress = "There is an error in the system, please try again";
                }

                    return RedirectToAction("index", "ProfileAddress");
            }
            else
            {
                return View();
            }
               
        }

        public ActionResult Edit(int id)
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            dataSet = webService.getAddressByID(id);
            Flower_recipient flower_Recipient = new Flower_recipient();
            flower_Recipient.Flower_recipient_id = int.Parse(dataSet.Tables[0].Rows[0]["Flower_recipient_id"].ToString());
            flower_Recipient.Name = dataSet.Tables[0].Rows[0]["Name"].ToString();
            flower_Recipient.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            flower_Recipient.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            flower_Recipient.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
            return View(flower_Recipient);
        }

        [HttpPost]
        public ActionResult Edit(Flower_recipient flower_Recipient)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.updateAddressByID(flower_Recipient.Flower_recipient_id, flower_Recipient.Name, flower_Recipient.Address, flower_Recipient.Phone);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAddress = "There is an error in the system, please try again";
                }

                return RedirectToAction("index", "ProfileAddress");
            }
            else
            {
                return View();
            }

        }

        public ActionResult Delete(int id)
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            dataSet = webService.getAddressByID(id);
            Flower_recipient flower_Recipient = new Flower_recipient();
            flower_Recipient.Flower_recipient_id = int.Parse(dataSet.Tables[0].Rows[0]["Flower_recipient_id"].ToString());
            flower_Recipient.Name = dataSet.Tables[0].Rows[0]["Name"].ToString();
            flower_Recipient.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            flower_Recipient.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            flower_Recipient.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
            return View(flower_Recipient);
        }

        [HttpPost]
        public ActionResult Delete(Flower_recipient flower_Recipient)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.deleteAddressByID(flower_Recipient.Flower_recipient_id);
                if (checkInsert == 0)
                {
                    ViewBag.bugDeleteAddress = "The address is not available now";
                }
                return RedirectToAction("index", "ProfileAddress");
            }
            catch (Exception)
            {
                Session["bugDeleteAddress"] = "The address is not available now";
                throw;
            }
                


        }

    }
}