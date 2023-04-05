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
    public class ManageEvaluteController : Controller
    {
        // GET: ManageSupplier
        // GET: ManageAccount
        public ActionResult Index()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllEvalute();
            List<Evalute> listEvalute = new List<Evalute>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Evalute Evalute = new Evalute();
                Evalute.Evalute_id = int.Parse(item["Evalute_id"].ToString());
                Evalute.Evalute_content = item["Evalute_content"].ToString();
                Evalute.Rate = int.Parse(item["Rate"].ToString());
                Evalute.Create_at = DateTime.Parse(item["Create_at"].ToString());
                Evalute.Customer_Name = item["First_name"].ToString() + " " + item["Last_name"].ToString();
                Evalute.Product_name = item["Product_name"].ToString();
                listEvalute.Add(Evalute);
            }
            ViewBag.Evalute = "Evalute";
            return View(listEvalute);
        }

        public ActionResult Delete(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllEvaluteByID((int)id);
            Evalute Evalute = new Evalute();
            Evalute.Evalute_id = int.Parse(dataSet.Tables[0].Rows[0]["Evalute_id"].ToString());
            Evalute.Evalute_content = dataSet.Tables[0].Rows[0]["Evalute_content"].ToString();
            Evalute.Rate = int.Parse(dataSet.Tables[0].Rows[0]["Rate"].ToString());
            Evalute.Create_at = DateTime.Parse(dataSet.Tables[0].Rows[0]["Create_at"].ToString());
            Evalute.Customer_Name = dataSet.Tables[0].Rows[0]["First_name"].ToString() + " " + dataSet.Tables[0].Rows[0]["Last_name"].ToString();
            Evalute.Product_name = dataSet.Tables[0].Rows[0]["Product_name"].ToString();
            ViewBag.Evalute = "Evalute";
            return View(Evalute);
        }
        [HttpPost]
        public ActionResult Delete(Evalute Evalute)
        {
            try
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.DeleteEvalute(Evalute.Evalute_id);
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