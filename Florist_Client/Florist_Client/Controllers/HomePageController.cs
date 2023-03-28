using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Index()
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            dataSet = webService.GetProductByCategory(1);
            List<Products> listProduct = new List<Products>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Products product = new Products();
                product.Product_id =  item["Product_id"].ToString();
                product.Product_name =  item["Product_name"].ToString();
                product.Description =  item["Description"].ToString();
                product.Image =  item["Image"].ToString();
                product.Price = float.Parse(item["Price"].ToString());
                product.Discount = float.Parse(item["Discount"].ToString());
                product.Category_name = item["Category_name"].ToString();
                product.Supplier_name = item["Supplier_name"].ToString();
                product.User_name = item["User_name"].ToString();
                product.Sold = int.Parse(item["Sold"].ToString());
                product.Inventory = int.Parse(item["Inventory"].ToString());
                product.Update_at =  DateTime.Parse(item["Update_at"].ToString());
                listProduct.Add(product);
            }
            ViewBag.Product1 = listProduct;

            dataSet = webService.GetProductByCategory(2);
            List<Products> listProduct2 = new List<Products>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Products product = new Products();
                product.Product_id = item["Product_id"].ToString();
                product.Product_name = item["Product_name"].ToString();
                product.Description = item["Description"].ToString();
                product.Image = item["Image"].ToString();
                product.Price = float.Parse(item["Price"].ToString());
                product.Discount = float.Parse(item["Discount"].ToString());
                product.Category_name = item["Category_name"].ToString();
                product.Supplier_name = item["Supplier_name"].ToString();
                product.User_name = item["User_name"].ToString();
                product.Sold = int.Parse(item["Sold"].ToString());
                product.Inventory = int.Parse(item["Inventory"].ToString());
                product.Update_at = DateTime.Parse(item["Update_at"].ToString());
                listProduct2.Add(product);
            }
            ViewBag.Product2 = listProduct2;


            dataSet = webService.GetProductByCategory(4);
            List<Products> listProduct4 = new List<Products>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Products product = new Products();
                product.Product_id = item["Product_id"].ToString();
                product.Product_name = item["Product_name"].ToString();
                product.Description = item["Description"].ToString();
                product.Image = item["Image"].ToString();
                product.Price = float.Parse(item["Price"].ToString());
                product.Discount = float.Parse(item["Discount"].ToString());
                product.Category_name = item["Category_name"].ToString();
                product.Supplier_name = item["Supplier_name"].ToString();
                product.User_name = item["User_name"].ToString();
                product.Sold = int.Parse(item["Sold"].ToString());
                product.Inventory = int.Parse(item["Inventory"].ToString());
                product.Update_at = DateTime.Parse(item["Update_at"].ToString());
                listProduct4.Add(product);
            }
            ViewBag.Product4 = listProduct4;

            dataSet = webService.GetProductByCategory(3);
            List<Products> listProduct3 = new List<Products>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Products product = new Products();
                product.Product_id = item["Product_id"].ToString();
                product.Product_name = item["Product_name"].ToString();
                product.Description = item["Description"].ToString();
                product.Image = item["Image"].ToString();
                product.Price = float.Parse(item["Price"].ToString());
                product.Discount = float.Parse(item["Discount"].ToString());
                product.Category_name = item["Category_name"].ToString();
                product.Supplier_name = item["Supplier_name"].ToString();
                product.User_name = item["User_name"].ToString();
                product.Sold = int.Parse(item["Sold"].ToString());
                product.Inventory = int.Parse(item["Inventory"].ToString());
                product.Update_at = DateTime.Parse(item["Update_at"].ToString());
                listProduct3.Add(product);
            }
            ViewBag.Product3 = listProduct3;
            if (Session["checklogin"] != null)
            {
                ViewBag.checkLogin = Session["checklogin"];
                Session["checklogin"] = null;
            }
            return View();
        }
    }
}
