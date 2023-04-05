using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Image_Client.Models;


namespace Image_Client.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            WCF.WebService api = new WCF.WebService();
            DataSet ds = new DataSet();

            ds = api.ShowAllProduct();

            List<Product> listpro = new List<Product>();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Product pr = new Product();
                pr.ProductID = (string)item["Product_id"];
                pr.ProductName = (string)item["Product_name"];
                pr.Description = (string)item["Description"];
                pr.Image = (string)item["Image"];
                pr.Price = float.Parse(item["Price"].ToString());
                pr.Discount = float.Parse(item["Discount"].ToString());
                pr.CategoryID = (int)item["Category_id"];
                pr.SupplierID = (int)item["Supplier_id"];
                pr.Sold = (int)item["Sold"];
                pr.Inventory = (int)item["Inventory"];
                //if (!Convert.IsDBNull(item["Picture"]))
                //{
                //    pr.Picture = (byte[])item["Picture"];
                //}



                listpro.Add(pr);
            }
            return View(listpro);
        }
    }
}