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