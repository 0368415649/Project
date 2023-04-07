using Florist_Client.Models;
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

        public ActionResult DetailsOrder(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetOrderDetailByDetailsID((int)id);
            OrderDetails orderDetails = new OrderDetails();
            orderDetails.Order_detail_id = int.Parse(dataSet.Tables[0].Rows[0]["Order_detail_id"].ToString());
            orderDetails.Product_id = dataSet.Tables[0].Rows[0]["Product_id"].ToString();
            orderDetails.Product_Name = dataSet.Tables[0].Rows[0]["Product_Name"].ToString();
            orderDetails.Order_id = int.Parse(dataSet.Tables[0].Rows[0]["Order_id"].ToString());
            orderDetails.Quantity = int.Parse(dataSet.Tables[0].Rows[0]["Quantity"].ToString());
            orderDetails.Price = float.Parse(dataSet.Tables[0].Rows[0]["Price"].ToString());
            orderDetails.Discount = float.Parse(dataSet.Tables[0].Rows[0]["Discount"].ToString());

            return View(orderDetails);
        }

        public ActionResult Details(int id)
        {
            DataSet dataSet = new DataSet();
            api_product.WebService webService = new api_product.WebService();
            dataSet = webService.GetOrderByID((int)id);
            Order order = new Order();
            order.Order_id = int.Parse(dataSet.Tables[0].Rows[0]["Order_id"].ToString());
            order.Status = dataSet.Tables[0].Rows[0]["Status"].ToString();
            order.Shipper_id = dataSet.Tables[0].Rows[0]["Shipper_id"].ToString() != "" ? int.Parse(dataSet.Tables[0].Rows[0]["Shipper_id"].ToString()) : 0;
            order.ShipperName = dataSet.Tables[0].Rows[0]["Shipper_id"].ToString() != "" ? dataSet.Tables[0].Rows[0]["Employee_name"].ToString() : "No one";
            order.Flower_recipient_id = int.Parse(dataSet.Tables[0].Rows[0]["Flower_recipient_id"].ToString());
            order.Name = dataSet.Tables[0].Rows[0]["Name"].ToString();
            order.Address = dataSet.Tables[0].Rows[0]["Address"].ToString();
            order.Phone = dataSet.Tables[0].Rows[0]["Phone"].ToString();
            order.Create_at = DateTime.Parse(dataSet.Tables[0].Rows[0]["Create_at"].ToString());
            order.Create_by = int.Parse(dataSet.Tables[0].Rows[0]["Create_by"].ToString());
            order.Create_Name = dataSet.Tables[0].Rows[0]["fullname"].ToString();
            order.Message_id = int.Parse(dataSet.Tables[0].Rows[0]["Message_id"].ToString());
            order.Content = dataSet.Tables[0].Rows[0]["Message_content"].ToString();
            order.Received_date = DateTime.Parse(dataSet.Tables[0].Rows[0]["Received_date"].ToString());
            order.Received_time = dataSet.Tables[0].Rows[0]["Received_time"].ToString();
            order.Total = float.Parse(dataSet.Tables[0].Rows[0]["Total"].ToString());

            return View(order);
        }
    }
}