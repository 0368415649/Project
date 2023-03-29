using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class CheckLoginController : Controller
    {
        // GET: CheckLogin
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            int count = webService.checkLoginCustomer(formCollection["login_phone"].ToString(), formCollection["login_password"].ToString());
            if(count > 0)
            {
                dataSet = webService.LoginCustomer(formCollection["login_phone"].ToString(), formCollection["login_password"].ToString());
                HttpCookie customerPhone = new HttpCookie("customerPhone");
                HttpCookie customerName = new HttpCookie("customerName");
                HttpCookie customerID = new HttpCookie("customerID");

                customerPhone.Value = formCollection["login_phone"].ToString();
                customerPhone.Expires = DateTime.MaxValue;
                customerName.Value = dataSet.Tables[0].Rows[0]["First_name"].ToString() +  " " + dataSet.Tables[0].Rows[0]["Last_name"].ToString();
                customerName.Expires = DateTime.MaxValue;
                customerID.Value = dataSet.Tables[0].Rows[0]["Customer_id"].ToString();
                customerID.Expires = DateTime.MaxValue;
                Response.Cookies.Add(customerPhone);
                Response.Cookies.Add(customerName);
                Response.Cookies.Add(customerID);
                /* Customer customer = new Customer();
                 customer.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
                 customer.First_name =dataSet.Tables[0].Rows[0]["First_name"].ToString();
                 customer.Last_name = dataSet.Tables[0].Rows[0]["Last_name"].ToString();
                 customer.Password = dataSet.Tables[0].Rows[0]["Password"].ToString();
                 customer.Sex = dataSet.Tables[0].Rows[0]["Sex"].ToString();
                 customer.Birth_day = DateTime.Parse(dataSet.Tables[0].Rows[0]["Birth_day"].ToString());
                 customer.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
                 customer.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
                 Session["customerName"] = customer;*/
                return RedirectToAction("Index", "HomePage");
            }
            else
            {
                Session["checklogin"] = "fail";
                return RedirectToAction("Index", "HomePage");
            }
        }
    }
}