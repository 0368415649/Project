﻿using Florist_Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Florist_Client.Controllers
{
    public class ManageOrderController : Controller
    {
        // GET: ManageOrder
        public ActionResult Index()
        {
            DataSet dataSetOrderdetail = new DataSet();

            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetAllOrder();
            List<Order> listOrder = new List<Order>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Order order = new Order();
                order.Order_id = int.Parse(item["Order_id"].ToString());
                order.Create_at = DateTime.Parse(item["Create_at"].ToString());
                order.Create_Name = item["fullname"].ToString();
                order.Received_date = DateTime.Parse(item["Received_date"].ToString());
                order.Received_time = item["Received_time"].ToString();
                order.Total =  float.Parse(item["Total"].ToString());
                order.Status =  item["Status"].ToString();
                dataSetOrderdetail = webService.GetOrderDetailByID(order.Order_id);
                List<OrderDetails> listOrderDetails = new List<OrderDetails>();
                foreach (DataRow i in dataSetOrderdetail.Tables[0].Rows)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.Order_detail_id = int.Parse(i["Order_detail_id"].ToString());
                    orderDetails.Product_id = i["Product_id"].ToString();
                    orderDetails.Product_Name = i["Product_Name"].ToString();
                    orderDetails.Order_id = int.Parse(i["Order_id"].ToString());
                    orderDetails.Quantity = int.Parse(i["Quantity"].ToString());
                    orderDetails.Price = float.Parse(i["Price"].ToString());
                    orderDetails.Discount = float.Parse(i["Discount"].ToString());
                    listOrderDetails.Add(orderDetails);
                }
                order.orderDetails = listOrderDetails;
                listOrder.Add(order);
            }
            ViewBag.order = "order";
            return View(listOrder);
        }
    }
}