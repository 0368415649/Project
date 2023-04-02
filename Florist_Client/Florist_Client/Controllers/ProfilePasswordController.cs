using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class ProfilePasswordController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            ViewBag.profilePass = "profilePass";
            return View();
        }
        [HttpPost]
        public ActionResult Index(ChangePass changePass)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.checkPasswordExits(changePass.OldPassword);
                if(checkInsert == 0)
                {
                    ViewBag.oldPass = "You entered the wrong old password!";
                    return View();
                }else if (!changePass.NewPassword.Equals(changePass.ComfirmPassword))
                {
                    ViewBag.oldPass = "Confirmation password is not correct!";
                    return View();
                }
                int cusID = 0;
                if (Request.Cookies["customerID"] != null)
                {
                    cusID = int.Parse(Request.Cookies["customerID"].Value);
                }
                int checkChangePass = webService.changePassword(cusID, changePass.NewPassword);
                return RedirectToAction("index", "Profile");
            }
                return View();
        }
    }
}