using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            string Inventory = webService.Inventory();
            string Alarm = webService.Alarm();
            string ExceedTheNorm = webService.ExceedTheNorm();
            string CountProduct = webService.CountProduct();
            string CountSupplier = webService.CountSupplier();
            string ProductUnclassified = webService.ProductUnclassified();
            string ProductsNotYetDiscounted = webService.ProductsNotYetDiscounted();
            string ProductNoSellingPriceYet = webService.ProductNoSellingPriceYet();
            string OrderNumber = webService.OrderNumber();
            string ProductNumber = webService.ProductNumber();
            string totalPrice = webService.MoneySell();
            ViewBag.Inventory = Inventory;
            ViewBag.Alarm = Alarm;
            ViewBag.ExceedTheNorm = ExceedTheNorm;
            ViewBag.CountSupplier = CountSupplier;
            ViewBag.CountProduct = CountProduct;
            ViewBag.ProductUnclassified = ProductUnclassified;
            ViewBag.ProductsNotYetDiscounted = ProductsNotYetDiscounted;
            ViewBag.ProductNoSellingPriceYet = ProductNoSellingPriceYet;
            ViewBag.OrderNumber = OrderNumber;
            ViewBag.ProductNumber = ProductNumber;
            ViewBag.totalPrice = totalPrice;
            ViewBag.Dashboard = "Dashboard";
            return View();
        }
    }
}