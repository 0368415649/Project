using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class LogoutPageController : Controller
    {
        // GET: LogoutPage
        public ActionResult Index()
        {
            HttpCookie customerPhone = new HttpCookie("customerPhone");
            HttpCookie customerName = new HttpCookie("customerName");
            HttpCookie customerID = new HttpCookie("customerID");
            customerPhone.Expires = DateTime.Now.AddDays(-1d);
            customerName.Expires = DateTime.Now.AddDays(-1d);
            customerID.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(customerName);
            Response.Cookies.Add(customerPhone);
            Response.Cookies.Add(customerID);
            return RedirectToAction("Index", "HomePage");
        }
    }
}