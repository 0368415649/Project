using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class ManageCategoryController : Controller
    {
        // GET: ManageSupplier
        // GET: ManageAccount
        public ActionResult Index()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllCategory();
            List<Category> listCategory = new List<Category>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Category category = new Category();
                category.Category_id = int.Parse(item["Category_id"].ToString());
                category.Category_name = item["Category_name"].ToString();
                listCategory.Add(category);
            }
            ViewBag.category = "category";
            return View(listCategory);
        }

        public ActionResult Create()
        {
            ViewBag.category = "category";
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.InsertCategory(category.Category_name);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.category = "category";
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetCategoryID((int)id);
            Category category = new Category();
            category.Category_id = int.Parse(dataSet.Tables[0].Rows[0]["Category_id"].ToString());
            category.Category_name = dataSet.Tables[0].Rows[0]["Category_name"].ToString();
            ViewBag.category = "category";
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.UpdateCategory(category.Category_id, category.Category_name);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.category = "category";
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetCategoryID((int)id);
            Category category = new Category();
            category.Category_id = int.Parse(dataSet.Tables[0].Rows[0]["Category_id"].ToString());
            category.Category_name = dataSet.Tables[0].Rows[0]["Category_name"].ToString();
            ViewBag.category = "category";
            return View(category);
        }
        [HttpPost]
        public ActionResult Delete(Category category)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.DeleteCategory(category.Category_id);
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