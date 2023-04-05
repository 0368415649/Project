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
    public class ManageSupplierController : Controller
    {
        // GET: ManageSupplier
        // GET: ManageAccount
        public ActionResult Index()
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
                supplier.Phone = item["Phone"].ToString();
                supplier.Address = item["Address"].ToString();
                listSupplier.Add(supplier);
            }
            ViewBag.Supplier = "Supplier";
            return View(listSupplier);
        }

        public ActionResult Create()
        {
            ViewBag.Supplier = "Supplier";
            return View();
        }
        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.InsertSupplier(supplier.Supplier_name, supplier.Address, supplier.Phone);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Supplier = "Supplier";
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllSupplierByID((int)id);
            Supplier supplier = new Supplier();
            supplier.Supplier_id = int.Parse(dataSet.Tables[0].Rows[0]["Supplier_id"].ToString());
            supplier.Supplier_name = dataSet.Tables[0].Rows[0]["Supplier_name"].ToString();
            supplier.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            supplier.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            ViewBag.Supplier = "Supplier";
            return View(supplier);
        }

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.UpdateSupplier(supplier.Supplier_id, supplier.Supplier_name, supplier.Address, supplier.Phone);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Supplier = "Supplier";
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllSupplierByID((int)id);
            Supplier supplier = new Supplier();
            supplier.Supplier_id = int.Parse(dataSet.Tables[0].Rows[0]["Supplier_id"].ToString());
            supplier.Supplier_name = dataSet.Tables[0].Rows[0]["Supplier_name"].ToString();
            supplier.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            supplier.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            ViewBag.Supplier = "Supplier";
            return View(supplier);
        }
        [HttpPost]
        public ActionResult Delete(Supplier supplier)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.DeleteSupplier(supplier.Supplier_id);
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