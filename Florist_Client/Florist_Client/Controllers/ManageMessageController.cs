using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class ManageMessageController : Controller
    {
        // GET: ManageMessage
        public ActionResult Index()
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllMessages();
            List<Messages> listMessages = new List<Messages>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Messages messages = new Messages();
                messages.Message_id = int.Parse(item["Message_id"].ToString());
                messages.Message_content = item["Message_content"].ToString();
                messages.Category = item["Category"].ToString();
                listMessages.Add(messages);
            }
            ViewBag.messageAdmin = "messageAdmin";
            return View(listMessages);
        }

        public ActionResult Create()
        {
            ViewBag.messageAdmin = "messageAdmin";
            return View();
        }
        [HttpPost]
        public ActionResult Create(Messages messages)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.createMessageAdmin(messages.Category, messages.Message_content);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.messageAdmin = "messageAdmin";
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllMessageByID(id);
            Messages messages = new Messages();
            messages.Message_id = int.Parse(dataSet.Tables[0].Rows[0]["Message_id"].ToString());
            messages.Message_content = dataSet.Tables[0].Rows[0]["Message_content"].ToString();
            messages.Category = dataSet.Tables[0].Rows[0]["Category"].ToString();
            ViewBag.messageAdmin = "messageAdmin";
            return View(messages);
        }

        [HttpPost]
        public ActionResult Edit(Messages messages)
        {
            if (ModelState.IsValid)
            {
                api_product.WebService webService = new api_product.WebService();
                int checkInsert = webService.updateMessageByID(messages.Message_id, messages.Message_content, messages.Category);
                if (checkInsert == 0)
                {
                    ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
                }
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.messageAdmin = "messageAdmin";
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllMessageByID(id);
            Messages messages = new Messages();
            messages.Message_id = int.Parse(dataSet.Tables[0].Rows[0]["Message_id"].ToString());
            messages.Message_content = dataSet.Tables[0].Rows[0]["Message_content"].ToString();
            messages.Category = dataSet.Tables[0].Rows[0]["Category"].ToString();
            ViewBag.messageAdmin = "messageAdmin";
            return View(messages);
        }

        [HttpPost]
        public ActionResult Delete(Messages messages)
        {
            api_product.WebService webService = new api_product.WebService();
            int checkInsert = webService.deleteMessageAdmin(messages.Message_id);
            if (checkInsert == 0)
            {
                ViewBag.bugInsertAccountAdmin = "There is an error in the system, please try again";
            }
            return RedirectToAction("index");
        }

    }
}