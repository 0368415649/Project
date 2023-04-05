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
    public class ManageAccountController : Controller
    {
        // GET: ManageAccount
        public ActionResult Index()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.getAllAdmin();
            List<AdminAccount> listAdminAccounts = new List<AdminAccount>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                AdminAccount adminAccount = new AdminAccount();
                adminAccount.Account_id = int.Parse(item["Account_id"].ToString());
                adminAccount.User_name = item["User_name"].ToString();
                adminAccount.Password = item["Password"].ToString();
                adminAccount.Group_id = int.Parse(item["Group_id"].ToString());
                adminAccount.Group_name =item["Group_name"].ToString();
                adminAccount.Employee_name = item["Employee_name"].ToString();
                adminAccount.Phone = item["Phone"].ToString();
                listAdminAccounts.Add(adminAccount);
            }
            ViewBag.account = "account";
            return View(listAdminAccounts);
        }

        public ActionResult Create()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.getAllGroup();
            List<AdminAccount> listGroup = new List<AdminAccount>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                AdminAccount adminAccount = new AdminAccount();
                adminAccount.Group_id = int.Parse(item["Group_id"].ToString());
                adminAccount.Group_name = item["Group_name"].ToString();
                listGroup.Add(adminAccount);
            }
            ViewBag.listGroup = new SelectList(listGroup, "Group_id", "Group_name");
            ViewBag.account = "account";
            return View();
        }
        [HttpPost]
        public ActionResult Create(AdminAccount account)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.insertAccountAdmin(account.User_name, account.Password, account.Group_id, account.Employee_name, account.Phone);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                DataSet dataSet = new DataSet();
                api_product.WebService webService = new api_product.WebService();
                dataSet = webService.getAllGroup();
                List<AdminAccount> listGroup = new List<AdminAccount>();
                foreach (DataRow item in dataSet.Tables[0].Rows)
                {
                    AdminAccount adminAccount = new AdminAccount();
                    adminAccount.Group_id = int.Parse(item["Group_id"].ToString());
                    adminAccount.Group_name = item["Group_name"].ToString();
                    listGroup.Add(adminAccount);
                }
                ViewBag.listGroup = new SelectList(listGroup, "Group_id", "Group_name");
                ViewBag.account = "account";
                return View();
            }
        }


        public ActionResult Edit(int? id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.getAllGroup();
            List<AdminAccount> listGroup = new List<AdminAccount>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                AdminAccount adminAccount = new AdminAccount();
                adminAccount.Group_id = int.Parse(item["Group_id"].ToString());
                adminAccount.Group_name = item["Group_name"].ToString();
                listGroup.Add(adminAccount);
            }
            ViewBag.listGroup = new SelectList(listGroup, "Group_id", "Group_name");
            ViewBag.account = "account";


            dataSet = webService.getAllAdminByID((int)id);
            AdminAccount adminEdit = new AdminAccount();
            adminEdit.Account_id = int.Parse(dataSet.Tables[0].Rows[0]["Account_id"].ToString());
            adminEdit.User_name = dataSet.Tables[0].Rows[0]["User_name"].ToString();
            adminEdit.Password = dataSet.Tables[0].Rows[0]["Password"].ToString();
            adminEdit.Group_id = int.Parse(dataSet.Tables[0].Rows[0]["Group_id"].ToString());
            adminEdit.Group_name = dataSet.Tables[0].Rows[0]["Group_name"].ToString();
            adminEdit.Employee_name = dataSet.Tables[0].Rows[0]["Employee_name"].ToString();
            adminEdit.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();            
            return View(adminEdit);
        }

        [HttpPost]
        public ActionResult Edit(AdminAccount account)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.updateAdminByID(account.Account_id, account.User_name, account.Password, account.Group_id, account.Employee_name, account.Phone);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                DataSet dataSet = new DataSet();
                api_product.WebService webService = new api_product.WebService();
                dataSet = webService.getAllGroup();
                List<AdminAccount> listGroup = new List<AdminAccount>();
                foreach (DataRow item in dataSet.Tables[0].Rows)
                {
                    AdminAccount adminAccount = new AdminAccount();
                    adminAccount.Group_id = int.Parse(item["Group_id"].ToString());
                    adminAccount.Group_name = item["Group_name"].ToString();
                    listGroup.Add(adminAccount);
                }
                ViewBag.listGroup = new SelectList(listGroup, "Group_id", "Group_name");
                ViewBag.account = "account";
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.getAllGroup();
            List<AdminAccount> listGroup = new List<AdminAccount>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                AdminAccount adminAccount = new AdminAccount();
                adminAccount.Group_id = int.Parse(item["Group_id"].ToString());
                adminAccount.Group_name = item["Group_name"].ToString();
                listGroup.Add(adminAccount);
            }
            ViewBag.listGroup = new SelectList(listGroup, "Group_id", "Group_name");
            ViewBag.account = "account";
            dataSet = webService.getAllAdminByID((int)id);
            AdminAccount adminEdit = new AdminAccount();
            adminEdit.Account_id = int.Parse(dataSet.Tables[0].Rows[0]["Account_id"].ToString());
            adminEdit.User_name = dataSet.Tables[0].Rows[0]["User_name"].ToString();
            adminEdit.Password = dataSet.Tables[0].Rows[0]["Password"].ToString();
            adminEdit.Group_id = int.Parse(dataSet.Tables[0].Rows[0]["Group_id"].ToString());
            adminEdit.Group_name = dataSet.Tables[0].Rows[0]["Group_name"].ToString();
            adminEdit.Employee_name = dataSet.Tables[0].Rows[0]["Employee_name"].ToString();
            adminEdit.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            return View(adminEdit);
        }
        [HttpPost]
        public ActionResult Delete(AdminAccount adminAccount)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.deleteAdminByAdmin(adminAccount.Account_id);
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