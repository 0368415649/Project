using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class ManageCustomerController : Controller
    {
        // GET: ManageCustomer
        public ActionResult Index()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllCustomer();
            List<Customer> listCustomer = new List<Customer>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Customer cus = new Customer();
                cus.Customer_id = int.Parse(item["Customer_id"].ToString());
                cus.First_name = item["First_name"].ToString();
                cus.Last_name = item["Last_name"].ToString();
                //cus.Password = item["Password"].ToString();
                cus.Sex = item["Sex"].ToString();
                if(item["Birth_day"].ToString() != "")
                {
                    cus.Birth_day =  DateTime.Parse(item["Birth_day"].ToString());
                }
                cus.Phone = item["Phone"].ToString();
                cus.Address = item["Address"].ToString();

                listCustomer.Add(cus);
            }
            ViewBag.customer = "customer";
            return View(listCustomer);
        }


        public ActionResult Edit(int? id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();

            dataSet = webService.GetCustomerByID((int)id);
            Customer customerEdit = new Customer();
            customerEdit.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
            customerEdit.First_name = dataSet.Tables[0].Rows[0]["First_name"].ToString();
            customerEdit.Sex = dataSet.Tables[0].Rows[0]["Sex"].ToString();
            customerEdit.Last_name = dataSet.Tables[0].Rows[0]["Last_name"].ToString();
            customerEdit.Password = dataSet.Tables[0].Rows[0]["Password"].ToString();
            customerEdit.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            customerEdit.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            ViewBag.customer = "customer";

            return View(customerEdit);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
   
                api_product.WebService webService = new api_product.WebService();
                int resultUpdate = webService.updateCustomerById(customer.Customer_id, customer.First_name, customer.Last_name, customer.Sex, customer.Birth_day, customer.Phone);
                if (resultUpdate == 0)
                {
                    Session["checkUpdate"] = "Update Fail, Please try again !";
                }
                ViewBag.customer = "customer";

                return RedirectToAction("Index");

            }
            ViewBag.customer = "customer";

            return View();
        }

        public ActionResult Delete(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();

            dataSet = webService.GetCustomerByID((int)id);
            Customer customerEdit = new Customer();
            customerEdit.Customer_id = int.Parse(dataSet.Tables[0].Rows[0]["Customer_id"].ToString());
            customerEdit.First_name = dataSet.Tables[0].Rows[0]["First_name"].ToString();
            customerEdit.Sex = dataSet.Tables[0].Rows[0]["Sex"].ToString();
            customerEdit.Last_name = dataSet.Tables[0].Rows[0]["Last_name"].ToString();
            customerEdit.Password = dataSet.Tables[0].Rows[0]["Password"].ToString();
            customerEdit.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            customerEdit.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            ViewBag.customer = "customer";

            return View(customerEdit);
        }
        [HttpPost]
        public ActionResult Delete(Customer cus)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.deleteCustomer(cus.Customer_id);
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