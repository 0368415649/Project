using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class DetailProductController : Controller
    {
        // GET: DetailProduct
        public ActionResult Index(string product_id, int? rate)
        {
            if(product_id == null || product_id == "")
            {
                return RedirectToAction("Index", "HomePage");
            }
            /* Detail product*/
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            dataSet = webService.GetProductByID(product_id);
            Products product = new Products();
            product.Product_id = dataSet.Tables[0].Rows[0]["Product_id"].ToString();
            product.Product_name = dataSet.Tables[0].Rows[0]["Product_name"].ToString();
            product.Description = dataSet.Tables[0].Rows[0]["Description"].ToString();
            product.Image = dataSet.Tables[0].Rows[0]["Image"].ToString();
            product.Price = float.Parse(dataSet.Tables[0].Rows[0]["Price"].ToString());
            product.Discount = float.Parse(dataSet.Tables[0].Rows[0]["Discount"].ToString());
            product.Category_name = dataSet.Tables[0].Rows[0]["Category_name"].ToString();
            product.Category_id = int.Parse(dataSet.Tables[0].Rows[0]["Category_id"].ToString());
            product.Supplier_name = dataSet.Tables[0].Rows[0]["Supplier_name"].ToString();
            product.User_name = dataSet.Tables[0].Rows[0]["User_name"].ToString();
            product.Sold = int.Parse(dataSet.Tables[0].Rows[0]["Sold"].ToString());
            product.Inventory = int.Parse(dataSet.Tables[0].Rows[0]["Inventory"].ToString());
            product.Update_at = DateTime.Parse(dataSet.Tables[0].Rows[0]["Update_at"].ToString());
            ViewBag.detailProduct = product;
            /* Detail product*/

            /* Category product*/
            dataSet = new DataSet();
            dataSet = webService.GetProductByCategory(product.Category_id);
            List<Products> listProduct = new List<Products>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Products product2 = new Products();
                product2.Product_id = item["Product_id"].ToString();
                product2.Product_name = item["Product_name"].ToString();
                product2.Description = item["Description"].ToString();
                product2.Image = item["Image"].ToString();
                product2.Price = float.Parse(item["Price"].ToString());
                product2.Discount = float.Parse(item["Discount"].ToString());
                product2.Category_name = item["Category_name"].ToString();
                product2.Supplier_name = item["Supplier_name"].ToString();
                product2.User_name = item["User_name"].ToString();
                product2.Sold = int.Parse(item["Sold"].ToString());
                product2.Inventory = int.Parse(item["Inventory"].ToString());
                product2.Update_at = DateTime.Parse(item["Update_at"].ToString());
                listProduct.Add(product2);
            }
            ViewBag.Product1 = listProduct;
            /* Category product*/

            /* Evalute*/
            rate = (rate == null) ? 0 : (int)rate;
            dataSet = new DataSet();
            dataSet = webService.GetEvaluteByProduct(product_id, (int)rate);
            List<Evalute> listEvalute = new List<Evalute>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Evalute Evalute = new Evalute();
                Evalute.Evalute_id = int.Parse(item["Evalute_id"].ToString());
                Evalute.Create_by = int.Parse(item["Create_by"].ToString());
                Evalute.Product_id = item["Product_id"].ToString();
                Evalute.Evalute_content = item["Evalute_content"].ToString();
                Evalute.Rate = int.Parse(item["Rate"].ToString());
                Evalute.Create_at = DateTime.Parse(item["Create_at"].ToString());
                Evalute.Customer_Name = item["Customer_Name"].ToString();
                Evalute.Product_name = item["Product_name"].ToString();
                listEvalute.Add(Evalute);
            }
            ViewBag.listEvalute = listEvalute;
            ViewBag.countRate1 = webService.GetCountEvaluteByRate(product_id, 1); 
            ViewBag.countRate2 = webService.GetCountEvaluteByRate(product_id, 2);
            ViewBag.countRate3 = webService.GetCountEvaluteByRate(product_id, 3);
            ViewBag.countRate4 = webService.GetCountEvaluteByRate(product_id, 4);
            ViewBag.countRate5 = webService.GetCountEvaluteByRate(product_id, 5);
            ViewBag.rate = rate;
            /* Evalute*/

            return View();
        }
    }
}