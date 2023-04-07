using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class AdminPagesController : Controller
    {
        // GET: AdminPage
        public ActionResult Index()
        {
            if(Session["checklogin"] != null)
            {
                ViewBag.loginFailAdmin = Session["checklogin"];
                Session["checklogin"] = null;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            int count = webService.checkLoginAdmin(formCollection["name"].ToString(), formCollection["pass"].ToString());
            if (count > 0)
            {
                dataSet = webService.LoginAccountAdmin(formCollection["name"].ToString(), formCollection["pass"].ToString());
                HttpCookie customerAccountID = new HttpCookie("AccountID");
                HttpCookie customerName = new HttpCookie("EmployeeName");
                HttpCookie customerGroupID = new HttpCookie("GroupId");
                customerAccountID.Value = dataSet.Tables[0].Rows[0]["Account_id"].ToString();
                customerAccountID.Expires = DateTime.MaxValue;
                customerGroupID.Value = dataSet.Tables[0].Rows[0]["Group_id"].ToString();
                customerGroupID.Expires = DateTime.MaxValue;
                customerName.Value = dataSet.Tables[0].Rows[0]["Employee_name"].ToString();
                customerName.Expires = DateTime.MaxValue;
                Response.Cookies.Add(customerAccountID);
                Response.Cookies.Add(customerName);
                Response.Cookies.Add(customerGroupID);
                dataSet = webService.getListUrl(int.Parse(dataSet.Tables[0].Rows[0]["Group_id"].ToString()));
                return RedirectToAction("Index", "ManageOrder");
            }
            else
            {
                 Session["checklogin"] = "fail";
                 return RedirectToAction("Index", "AdminPages");
            }
        }
    }
}