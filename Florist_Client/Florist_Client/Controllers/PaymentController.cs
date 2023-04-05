using Florist_Client.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Florist_Client.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            ViewBag.payment = "payment";
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            api_product.WebService webService = new api_product.WebService();
            Flower_recipient flower_Recipient_buy = (Flower_recipient)Session["flower_Recipient"];
            int recipientID = flower_Recipient_buy.Flower_recipient_id;
            int cusID = 0;
            if (Request.Cookies["customerID"] != null)
            {
                cusID = int.Parse(Request.Cookies["customerID"].Value);
            }
            /* create Flower_recipient*/
            if (recipientID == 0)
            {
                webService.insertAddressByCustomer(cusID, flower_Recipient_buy.Name, flower_Recipient_buy.Address, flower_Recipient_buy.Phone);
                recipientID = webService.getAddressMax();
            }
            /* create Flower_recipient*/
            /* create Message*/
            string message = Session["message"].ToString();
            webService.createMessage(cusID, message);
            int messageID = webService.getMessageMax();
            /* create Message*/
            string date = Session["date"].ToString();
            string time = Session["time"].ToString();
            float total = float.Parse(formCollection["card_total"].ToString());
            /* create Order*/
            webService.insertOrder(cusID, recipientID, total, messageID, date, time);
            int order_id = webService.getOrderMax();
            /* create Order*/
            JavaScriptSerializer js = new JavaScriptSerializer();
            ProductsCartAll[] listProductCart = js.Deserialize<ProductsCartAll[]>(formCollection["card_submit"].ToString());
            /* create OrderDetails*/
            foreach (var item in listProductCart)
            {
                webService.insertOrderDetails(item.product.id, order_id, item.quantity, item.product.price, item.product.discount);
            }
            /* create OrderDetails*/

            /* create Bankking*/
            if (formCollection["card_num"] != "" || formCollection["card_name"] != "")

                webService.insertBanking(formCollection["card_name"].ToString(), formCollection["card_num"].ToString(), cusID, order_id, total);
            /* create Bankking*/
            Session["removeCart"] = "remove";
            return RedirectToAction("index", "HomePage");
        }
    }
}