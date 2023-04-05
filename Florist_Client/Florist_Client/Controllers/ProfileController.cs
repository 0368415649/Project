using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Florist_Client.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            int cusID = 0;
            if (Request.Cookies["customerID"] != null)
            {
                cusID = int.Parse(Request.Cookies["customerID"].Value);
            }
            dataSet = webService.getInformationCustomer(cusID);
            Customer customer = new Customer();
            customer.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
            customer.First_name = dataSet.Tables[0].Rows[0]["First_name"].ToString();
            customer.Last_name = dataSet.Tables[0].Rows[0]["Last_name"].ToString();
            customer.Sex = dataSet.Tables[0].Rows[0]["Sex"].ToString();
            customer.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            if(dataSet.Tables[0].Rows[0]["Birth_day"].ToString() != "")
            {
                customer.Birth_day =  DateTime.Parse(dataSet.Tables[0].Rows[0]["Birth_day"].ToString());
            }
            if (Session["checkUpdate"] != null)
            {
                ViewBag.updateFail = Session["checkUpdate"];
                Session["checkUpdate"] = null;
            }
            ViewBag.profile = "profile";
            return View(customer);
        }

        [HttpPost]
        public ActionResult Index(Customer customer)
        {
            if (ModelState.IsValid)
            {
                string oldphone = "";
                if (Request.Cookies["customerPhone"] != null)
                {
                    oldphone = Request.Cookies["customerPhone"].Value;
                }
                api_product.WebService webService = new api_product.WebService();
                int checkPhone = webService.checkPhoneExits(oldphone, customer.Phone);
                if(checkPhone != 0)
                {
                    Session["checkUpdate"] = "Phone is exits, Please try again !";
                }
                else
                {
                    int resultUpdate = webService.updateCustomerById(customer.Customer_id, customer.First_name, customer.Last_name, customer.Sex, customer.Birth_day, customer.Phone);
                    if (resultUpdate == 0)
                    {
                        Session["checkUpdate"] = "Update Fail, Please try again !";
                    }
                    HttpCookie customerPhone = new HttpCookie("customerPhone");
                    HttpCookie customerName = new HttpCookie("customerName");
                    customerPhone.Value = customer.Phone;
                    customerPhone.Expires = DateTime.MaxValue;
                    customerName.Value = customer.First_name + " " + customer.Last_name;
                    customerName.Expires = DateTime.MaxValue;
                    Response.Cookies.Add(customerPhone);
                    Response.Cookies.Add(customerName);
                }
                return RedirectToAction("Index");

            }
            return View();
        }
    }
}