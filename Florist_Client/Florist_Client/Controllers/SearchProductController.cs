using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
   
    public class SearchProductController : Controller
    {
        public void calc()
        {

        }
        // GET: SearchProduct
        public ActionResult Index(string searchName, string sortName, string typeSort, int? currenPage, int? category)
        {

            searchName = (searchName == null) ? "": searchName;
            sortName = (sortName == null) ? "Product_id" : sortName;
            typeSort = (typeSort == null) ? "asc": typeSort;
            int limit = 8;
            currenPage = (currenPage == null) ? 1 : int.Parse(currenPage.ToString());
            int offset = (int)(currenPage-1) * 8;
            category = (category == null) ? 0 : (int)category;
            api_product.WebService webService = new api_product.WebService();
            int countRecord = webService.GetCountProduct(searchName, (int)category);
            int totalPage = 0;
            if(countRecord > 0)
            {
                int numberDrive = countRecord % limit;
                if(numberDrive > 0)
                {
                    totalPage = (countRecord / limit) + 1;
                }
                else
                {
                    totalPage = (countRecord / limit);
                }
            }
            string category_title = "";
            if(category > 0)
            {
                category_title += webService.GetCategoryName((int)category);
            }
            if (searchName != null && searchName != "")
            {
                category_title = searchName;
            }
            DataSet dataSet = new DataSet();
            dataSet = webService.GetAllProduct(searchName, sortName, typeSort, offset, limit, (int)category);
            List<Products> listProduct = new List<Products>();
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
                listProduct.Add(product);
            }
            ViewBag.Product1 = listProduct;
            ViewBag.categoryHover = category;
            ViewBag.searchName = searchName;
            ViewBag.totalPage = totalPage;
            ViewBag.currenPage = currenPage;
            ViewBag.sortName = sortName;
            ViewBag.typeSort = typeSort;
            ViewBag.category_title = category_title;
      
            return View();
        }
    }
}