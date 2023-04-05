using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class RegistrationPageController : Controller
    {
        // GET: RegistrationPage
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            string  number = formCollection["phone"];
            string first_name = formCollection["first_name"];
            string last_name = formCollection["last_name"];
            string password = formCollection["password"];
            int checkPhone = webService.checkRegistrationCustomer(number);
            if(checkPhone > 0)
            {
                Session["checkRegistration"] = "fail";
                return RedirectToAction("Index", "HomePage");
            }
            webService.registrationCustomer(first_name, last_name, number, password);
            Session["registrationOk"] = "ok";

            return RedirectToAction("Index", "HomePage");
        }
    }
}