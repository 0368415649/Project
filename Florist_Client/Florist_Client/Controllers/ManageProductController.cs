using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Florist_Client.Controllers
{
    public class ManageProductController : Controller
    {
        // GET: ManageAccount
        public ActionResult Index()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllProducts();
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
                product.Category_id = int.Parse(item["Category_id"].ToString());
                product.Category_name = item["Category_name"].ToString();
                product.Supplier_id = int.Parse(item["Supplier_id"].ToString());
                product.Supplier_name = item["Supplier_name"].ToString();
                product.User_name = item["User_name"].ToString();
                product.Update_by = int.Parse(item["Update_by"].ToString());
                product.Sold = int.Parse(item["Sold"].ToString());
                product.Inventory = int.Parse(item["Inventory"].ToString());
                product.Update_at = DateTime.Parse(item["Update_at"].ToString());
                listProduct.Add(product);
            }
            ViewBag.product = "product";
            return View(listProduct);
        }

        public ActionResult Details(string id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllProductsID(id.ToString());
            Products product = new Products();
            product.Product_id = dataSet.Tables[0].Rows[0]["Product_id"].ToString();
            product.Product_name = dataSet.Tables[0].Rows[0]["Product_name"].ToString();
            product.Description = dataSet.Tables[0].Rows[0]["Description"].ToString();
            product.Image = dataSet.Tables[0].Rows[0]["Image"].ToString();
            product.Price = float.Parse(dataSet.Tables[0].Rows[0]["Price"].ToString());
            product.Discount = float.Parse(dataSet.Tables[0].Rows[0]["Discount"].ToString());
            product.Category_id = int.Parse(dataSet.Tables[0].Rows[0]["Category_id"].ToString());
            product.Category_name = dataSet.Tables[0].Rows[0]["Category_name"].ToString();
            product.Supplier_id = int.Parse(dataSet.Tables[0].Rows[0]["Supplier_id"].ToString());
            product.Supplier_name = dataSet.Tables[0].Rows[0]["Supplier_name"].ToString();
            product.User_name = dataSet.Tables[0].Rows[0]["User_name"].ToString();
            product.Update_by = int.Parse(dataSet.Tables[0].Rows[0]["Update_by"].ToString());
            product.Sold = int.Parse(dataSet.Tables[0].Rows[0]["Sold"].ToString());
            product.Inventory = int.Parse(dataSet.Tables[0].Rows[0]["Inventory"].ToString());
            product.Update_at = DateTime.Parse(dataSet.Tables[0].Rows[0]["Update_at"].ToString());
            ViewBag.product = "product";
            return View(product);
        }


        public ActionResult Create()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllSupplier();
            List<Supplier> listSupplier = new List<Supplier>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Supplier supplier = new Supplier();
                supplier.Supplier_id = int.Parse(item["Supplier_id"].ToString());
                supplier.Supplier_name = item["Supplier_name"].ToString();
                listSupplier.Add(supplier);
            }
            ViewBag.listSupplier = new SelectList(listSupplier, "Supplier_id", "Supplier_name");

            dataSet = webService.GetAllCategory();
            List<Category> listCategory = new List<Category>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Category category = new Category();
                category.Category_id = int.Parse(item["Category_id"].ToString());
                category.Category_name = item["Category_name"].ToString();
                listCategory.Add(category);
            }
            ViewBag.listCategory = new SelectList(listCategory, "Category_id", "Category_name");

            ViewBag.product = "product";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Products products, HttpPostedFileBase image)
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            ViewBag.product = "product";
            dataSet = webService.GetAllSupplier();
            List<Supplier> listSupplier = new List<Supplier>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Supplier supplier = new Supplier();
                supplier.Supplier_id = int.Parse(item["Supplier_id"].ToString());
                supplier.Supplier_name = item["Supplier_name"].ToString();
                listSupplier.Add(supplier);
            }
            ViewBag.listSupplier = new SelectList(listSupplier, "Supplier_id", "Supplier_name");

            dataSet = webService.GetAllCategory();
            List<Category> listCategory = new List<Category>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Category category = new Category();
                category.Category_id = int.Parse(item["Category_id"].ToString());
                category.Category_name = item["Category_name"].ToString();
                listCategory.Add(category);
            }
            ViewBag.listCategory = new SelectList(listCategory, "Category_id", "Category_name");
            if (ModelState.IsValid)
            {
                int checkExitsProduct = webService.checkExitsProduct(products.Product_id);
                if (checkExitsProduct > 0)
                {
                    ViewBag.productExits = "productExits";
                    return View();
                    
                }
                int accountID = 0;
                if (Request.Cookies["AccountID"] != null)
                {
                    accountID = int.Parse(Request.Cookies["AccountID"].Value);
                }
                string imageUrl = Server.MapPath("~/Resources/img/product/" + products.Product_id +".png");
                webService.insertGetProductByID(products.Product_id, products.Product_name, products.Description, products.Product_id,
                products.Price, products.Discount, products.Category_id, products.Supplier_id, accountID, products.Inventory);
                image.SaveAs(imageUrl);
                return RedirectToAction("index");
            }
            else
            {             
                ViewBag.product = "product";
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            ViewBag.product = "product";
            dataSet = webService.GetAllSupplier();
            List<Supplier> listSupplier = new List<Supplier>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Supplier supplier = new Supplier();
                supplier.Supplier_id = int.Parse(item["Supplier_id"].ToString());
                supplier.Supplier_name = item["Supplier_name"].ToString();
                listSupplier.Add(supplier);
            }
            ViewBag.listSupplier = new SelectList(listSupplier, "Supplier_id", "Supplier_name");

            dataSet = webService.GetAllCategory();
            List<Category> listCategory = new List<Category>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Category category = new Category();
                category.Category_id = int.Parse(item["Category_id"].ToString());
                category.Category_name = item["Category_name"].ToString();
                listCategory.Add(category);
            }
            ViewBag.listCategory = new SelectList(listCategory, "Category_id", "Category_name");
            dataSet = webService.GetAllProductsID(id.ToString());
            Products product = new Products();
            product.Product_id = dataSet.Tables[0].Rows[0]["Product_id"].ToString();
            product.Product_name = dataSet.Tables[0].Rows[0]["Product_name"].ToString();
            product.Description = dataSet.Tables[0].Rows[0]["Description"].ToString();
            product.Image = dataSet.Tables[0].Rows[0]["Image"].ToString();
            product.Price = float.Parse(dataSet.Tables[0].Rows[0]["Price"].ToString());
            product.Discount = float.Parse(dataSet.Tables[0].Rows[0]["Discount"].ToString());
            product.Category_id = int.Parse(dataSet.Tables[0].Rows[0]["Category_id"].ToString());
            product.Category_name = dataSet.Tables[0].Rows[0]["Category_name"].ToString();
            product.Supplier_id = int.Parse(dataSet.Tables[0].Rows[0]["Supplier_id"].ToString());
            product.Supplier_name = dataSet.Tables[0].Rows[0]["Supplier_name"].ToString();
            product.User_name = dataSet.Tables[0].Rows[0]["User_name"].ToString();
            product.Update_by = int.Parse(dataSet.Tables[0].Rows[0]["Update_by"].ToString());
            product.Sold = int.Parse(dataSet.Tables[0].Rows[0]["Sold"].ToString());
            product.Inventory = int.Parse(dataSet.Tables[0].Rows[0]["Inventory"].ToString());
            product.Update_at = DateTime.Parse(dataSet.Tables[0].Rows[0]["Update_at"].ToString());
            ViewBag.product = "product";
            return View(product);
            
        }

        [HttpPost]
        public ActionResult Edit(Products products, HttpPostedFileBase image)
        {
            api_product.WebService webService = new api_product.WebService();
            DataSet dataSet = new DataSet();
            ViewBag.product = "product";
            dataSet = webService.GetAllSupplier();
            List<Supplier> listSupplier = new List<Supplier>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Supplier supplier = new Supplier();
                supplier.Supplier_id = int.Parse(item["Supplier_id"].ToString());
                supplier.Supplier_name = item["Supplier_name"].ToString();
                listSupplier.Add(supplier);
            }
            ViewBag.listSupplier = new SelectList(listSupplier, "Supplier_id", "Supplier_name");

            dataSet = webService.GetAllCategory();
            List<Category> listCategory = new List<Category>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Category category = new Category();
                category.Category_id = int.Parse(item["Category_id"].ToString());
                category.Category_name = item["Category_name"].ToString();
                listCategory.Add(category);
            }
            ViewBag.listCategory = new SelectList(listCategory, "Category_id", "Category_name");
            if (ModelState.IsValid)
            {              
                int accountID = 0;
                if (Request.Cookies["AccountID"] != null)
                {
                    accountID = int.Parse(Request.Cookies["AccountID"].Value);
                }
                products.Description = products.Description.Replace("'", "''");
                products.Description = products.Description.Replace("‘", "''");
                products.Description = products.Description.Replace("’", "''");
                string imageUrl = Server.MapPath("~/Resources/img/product/" + products.Product_id + ".png");
                webService.updateProductByID(products.Product_id, products.Product_name, products.Description, products.Product_id,
                products.Price, products.Discount, products.Category_id, products.Supplier_id, accountID, products.Inventory, products.Sold);
                if(image != null && image.ContentLength > 0)
                {
                    image.SaveAs(imageUrl);
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.product = "product";
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllProductsID(id.ToString());
            Products product = new Products();
            product.Product_id = dataSet.Tables[0].Rows[0]["Product_id"].ToString();
            product.Product_name = dataSet.Tables[0].Rows[0]["Product_name"].ToString();
            product.Description = dataSet.Tables[0].Rows[0]["Description"].ToString();
            product.Image = dataSet.Tables[0].Rows[0]["Image"].ToString();
            product.Price = float.Parse(dataSet.Tables[0].Rows[0]["Price"].ToString());
            product.Discount = float.Parse(dataSet.Tables[0].Rows[0]["Discount"].ToString());
            product.Category_id = int.Parse(dataSet.Tables[0].Rows[0]["Category_id"].ToString());
            product.Category_name = dataSet.Tables[0].Rows[0]["Category_name"].ToString();
            product.Supplier_id = int.Parse(dataSet.Tables[0].Rows[0]["Supplier_id"].ToString());
            product.Supplier_name = dataSet.Tables[0].Rows[0]["Supplier_name"].ToString();
            product.User_name = dataSet.Tables[0].Rows[0]["User_name"].ToString();
            product.Update_by = int.Parse(dataSet.Tables[0].Rows[0]["Update_by"].ToString());
            product.Sold = int.Parse(dataSet.Tables[0].Rows[0]["Sold"].ToString());
            product.Inventory = int.Parse(dataSet.Tables[0].Rows[0]["Inventory"].ToString());
            product.Update_at = DateTime.Parse(dataSet.Tables[0].Rows[0]["Update_at"].ToString());
            ViewBag.product = "product";
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(Products products)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.deleteProductsByID(products.Product_id);
                if (checkInsert == 0)
                {
                    ViewBag.bugDeleteAddress = "The address is not available now";
                }
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                Session["bugDeleteAddress"] = "The address is not available now";
                throw;
            }



        }


    }
}