﻿using System;
using System.Activities.Expressions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    string connstr = ConfigurationManager.ConnectionStrings["florist"].ConnectionString;
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    // ------------  API For Message Table -----------

    ////1. Api get all message
    [WebMethod]
    public DataSet GetAllMessage(int Customer_id)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Message_id, Message_content, Category, Customer_id FROM [FloristDB].[dbo].[Message] Where Customer_id IS NULL ";
        if(Customer_id != 0)
        {
            sql += "OR Customer_id = " + Customer_id;
        }
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }
    [WebMethod]
    public int createMessage(int Customer_id, string Message_content)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO [FloristDB].[dbo].[Message] (Message_content ,Customer_id) " +
                " VALUES( N'" + Message_content + "', " +
                + Customer_id + ")";

            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }

    [WebMethod]
    public int getMessageMax()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Max(Message_id) as max  " +
            " FROM [FloristDB].[dbo].[Message] ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["max"].ToString());
    }


    // 2. Api get Message_content by Category
    [WebMethod]
    public DataSet GetMessageByCategory(string CategoryName)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Message_content FROM Message WHERE Category = '" + CategoryName + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // -----------------API For Product font end Table -------------

    //1. Api getcount product by searchName,category
    [WebMethod]
    public int GetCountProduct(string searchName, int category)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string queryCategory = "";
        if (category != 0)
        {
            queryCategory = "AND Category.Category_id = " + category;
        }
        string sql = "SELECT Count(*) as count FROM     Product" +
            " LEFT OUTER JOIN    Category ON Product.Category_id = Category.Category_id" +
            " LEFT OUTER JOIN  Account ON Product.Update_by = Account.Account_id LEFT OUTER JOIN  Supplier ON Product.Supplier_id = Supplier.Supplier_id  " +
            " Where  Product.Product_name LIKE '%" + searchName + "%' " + queryCategory;

        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }

    //2. Api get All Product by search, page, sort 
    [WebMethod]
    public DataSet GetAllProduct(string searchName, string sortName, string typeSort, int offset, int limit, int category)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string queryCategory = "";
        if (category != 0)
        {
            queryCategory = "AND Category.Category_id = " + category;
        }
        string sql = "SELECT Product.Product_id, Product.Product_name, Product.Description," +
            " Product.Image, Product.Price, Product.Discount, Category.Category_name," +
            " Supplier.Supplier_name, Account.Account_id, Account.User_name, Product.Sold," +
            " Product.Inventory, Product.Update_at FROM     Product" +
            " LEFT OUTER JOIN    Category ON Product.Category_id = Category.Category_id" +
            " LEFT OUTER JOIN  Account ON Product.Update_by = Account.Account_id LEFT OUTER JOIN  Supplier ON Product.Supplier_id = Supplier.Supplier_id  " +
            " Where  Product.Product_name LIKE '%" + searchName + "%' " + queryCategory +
            " ORder by " + sortName + " " + typeSort +
            " OFFSET " + offset + " ROWS " +
            " FETCH NEXT  " + limit + "  ROWS ONLY";

        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //3. Api get All Product by category
    [WebMethod]
    public DataSet GetProductByCategory(int categoryID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Top(4) Product.Product_id, Product.Product_name, Product.Description," +
            " Product.Image, Product.Price, Product.Discount, Category.Category_name," +
            " Supplier.Supplier_name, Account.Account_id, Account.User_name, Product.Sold," +
            " Product.Inventory, Product.Update_at FROM     Product" +
            " LEFT OUTER JOIN    Category ON Product.Category_id = Category.Category_id" +
            " LEFT OUTER JOIN  Account ON Product.Update_by = Account.Account_id LEFT OUTER JOIN  Supplier ON Product.Supplier_id = Supplier.Supplier_id " +
            " Where  Category.Category_id = " + categoryID;

        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //4. Api get All Product by id(
    [WebMethod]
    public DataSet GetProductByID(string productID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Product.Product_id, Product.Product_name, Product.Description," +
            " Product.Image, Product.Price, Product.Discount, Category.Category_id, Category.Category_name," +
            " Supplier.Supplier_name, Account.Account_id, Account.User_name, Product.Sold," +
            " Product.Inventory, Product.Update_at FROM     Product" +
            " LEFT OUTER JOIN    Category ON Product.Category_id = Category.Category_id" +
            " LEFT OUTER JOIN  Account ON Product.Update_by = Account.Account_id LEFT OUTER JOIN  Supplier ON Product.Supplier_id = Supplier.Supplier_id " +
            " Where  Product.Product_id = '" + productID + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }



    //1. Api get All Product by search, page, sort 
    [WebMethod]
    public string GetCategoryName(int category)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Category_name FROM [FloristDB].[dbo].[Category]  Where  Category.Category_id = " + category;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds.Tables[0].Rows[0]["Category_name"].ToString();
    }

    //// ------------------API For Customer Table -----------------

    // 1. Api Login Account Table
    [WebMethod]
    public DataSet LoginAccount(string Phone, string Password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT  Account.User_name,  Account.Password,  Account.Account_id,  Account.Employee_name,  Account.Phone,  [Group].Group_name,  [Function].Function_name,  [Function].Base_url FROM      Account LEFT OUTER JOIN  [Group] ON  Account.Group_id =  [Group].Group_id FULL OUTER JOIN  Group_function FULL OUTER JOIN  [Function] ON  Group_function.Function_id =  [Function].Function_id ON  [Group].Group_id =  Group_function.Group_id WHERE Account.Phone = N'" + Phone + "' AND Account.Password = N'" + Password + "'";
        //string sql = "SELECT * FROM Account WHERE Phone = '" + Phone + "' AND Password = '" + Password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. Api Login Customer Table
    [WebMethod]
    public DataSet LoginCustomer(string Phone, string Password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Customer_id, First_name, Last_name, Password, Sex, Birth_day, Phone, Address FROM  [FloristDB].[dbo].[Customer] WHERE Phone = N'" + Phone + "'AND Password = N'" + Password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public int checkLoginCustomer(string Phone, string Password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Count(*) as count FROM  [FloristDB].[dbo].[Customer] WHERE Phone = N'" + Phone + "'AND Password = N'" + Password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }

    ////3. Api Register Customer Table
    //[WebMethod]
    //public int LoginCustomer(string FirstName, string LastName, string Phone, string Password)
    //{
    //    SqlConnection cnn = new SqlConnection(connstr);
    //    cnn.Open();
    //    SqlCommand cmd = new SqlCommand();
    //    cmd.Connection = cnn;
    //    string sql = "INSERT INTO Customer (First_name, Last_name, Phone, Password) VALUES (N'" + FirstName + "',N'" + LastName + "',N'" + Phone + "',N'" + Password + "')";
    //    cmd.CommandText = sql;
    //    cmd.CommandType = CommandType.Text;
    //    cmd.ExecuteNonQuery();
    //    return 1;
    //}

    [WebMethod]
    public DataSet getInformationCustomer(int customerID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT *  FROM  [FloristDB].[dbo].[Customer] WHERE Customer_id = " + customerID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public int checkPhoneExits(string oldPhone, string newphone)
    {
        if (oldPhone.Equals(newphone))
        {
            return 0;
        }
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT  Count(*) as count  FROM  [FloristDB].[dbo].[Customer] WHERE  Phone = N'" + newphone + "'" ;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }

    [WebMethod]
    public int checkPasswordExits(string password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT  Count(*) as count  FROM  [FloristDB].[dbo].[Customer] WHERE  Password = N'" + password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }

    [WebMethod]
    public int changePassword(int customerID,  string password)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "UPDATE [FloristDB].[dbo].[Customer] SET " +
                " Password = N'" + password + "' " +
                " WHERE Customer_id = " + customerID;

            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }

    [WebMethod]
    public int updateCustomerById(int customerID, string First_name, string Last_name, string Sex, DateTime Birth_day, string Phone)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "UPDATE [FloristDB].[dbo].[Customer] SET " +
                " First_name = N'" + First_name + "', " +
                " Last_name = N'" + Last_name + "', " +
                " Sex = N'" + Sex + "', " +
                " Birth_day = N'" + Birth_day + "', " +
                " Phone = N'" + Phone + "' " +
                " WHERE Customer_id = " + customerID;

            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }

    //// ------------------API For Customer Table -----------------

    //1.0 API get All Evalute Table
    [WebMethod]
    public DataSet GetAllEvalute()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute.Evalute_id, Evalute.Evalute_content, Evalute.Rate, Evalute.Create_at, Product.Product_id, Customer.Customer_id, Customer.First_name, Customer.Last_name, Product.Product_name FROM     Evalute LEFT OUTER JOIN    Customer ON Evalute.Create_by = Customer.Customer_id LEFT OUTER JOIN    Product ON Evalute.Product_id = Product.Product_id";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //1.Api get Evalute by Product
    [WebMethod]
    public DataSet GetEvaluteByProduct(string ProductID, int? rate)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute.Evalute_id, Evalute.Evalute_content, Evalute.Create_by , Evalute.Rate," +
            " Evalute.Create_at, Product.Product_id, Customer.Customer_id," +
            " CONCAT(Customer.First_name,' ', Customer.Last_name) as Customer_Name, Product.Product_name" +
            " FROM     Evalute " +
            "LEFT OUTER JOIN    Customer ON Evalute.Create_by = Customer.Customer_id " +
            "LEFT OUTER JOIN    Product ON Evalute.Product_id = Product.Product_id " +
            "WHERE Evalute.Product_id = N'" + ProductID + "'";
        if(rate != 0)   
        {
            sql += "AND Evalute.Rate = " + rate;
        }
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API get Evalute by Rate
    [WebMethod]
    public int GetCountEvaluteByRate(string ProductID,int Rate)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Count(*) as count FROM Evalute" +
            " WHERE Evalute.Product_id = N'" + ProductID + "' And Rate = " + Rate;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }


    //// ------------------API For Flower_recipient Table -----------------
    [WebMethod]
    public DataSet getAddressByCustomer(int customerID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Flower_recipient_id, Name, Address, Phone, Customer_id " +
            " FROM Flower_recipient WHERE Customer_id = " + customerID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet getAddressByID(int ID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Flower_recipient_id, Name, Address, Phone, Customer_id " +
            " FROM Flower_recipient WHERE Flower_recipient_id = " + ID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public int getAddressMax()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Max(Flower_recipient_id) as max  " +
            " FROM Flower_recipient ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["max"].ToString());
    }

    [WebMethod]
     public int insertAddressByCustomer(int customerID, string Name, string Address, string Phone) { 
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO Flower_recipient (Name , Address , Phone ,Customer_id) " +
                " VALUES( N'" + Name + "', " +
                " N'" + Address + "', " +
                " N'" + Phone + "', " +
                + customerID  + ")";

            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }

    [WebMethod]
    public int updateAddressByID(int Flower_recipient_id, string Name, string Address, string Phone)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE Flower_recipient SET ");
            sql.Append(" Name = N'" + Name + "', ");
            sql.Append(" Address = N'" + Address + "', ");
            sql.Append(" Phone  =  N'" + Phone + "' ");
            sql.Append(" WHERE  Flower_recipient_id  =  " + Flower_recipient_id );

            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }


    [WebMethod]
    public int deleteAddressByID(int Flower_recipient_id)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            StringBuilder sql = new StringBuilder();
            sql.Append(" DELETE FROM Flower_recipient ");
            sql.Append(" WHERE  Flower_recipient_id  =  " + Flower_recipient_id);

            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {
            return 0;
            throw;
        }
        return 0;

    }


    //// ------------------API For Flower_recipient Table -----------------
    ///

    //// ------------------API For Order Table -----------------


    [WebMethod]
    public int insertOrder(int customerID, int Flower_recipient_id, int Total, int Message_id, string Received_date, string Received_time)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO [FloristDB].[dbo].[Order] (Status , Flower_recipient_id , Total, Create_at, Create_by ,Message_id, Received_date, Received_time) " +
                " VALUES( N'Pending', " +
                + Flower_recipient_id + " , " +
                + Total + " , " +
                " GETDATE() , " +
                + customerID  +" , " +
                + Message_id + " , " +
                " N'" + Received_date + "', " +
                " N'" + Received_time + "' )";
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }

    [WebMethod]
    public int getOrderMax()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Max(Order_id) as max  " +
            " FROM [FloristDB].[dbo].[Order] ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["max"].ToString());
    }

    //// ------------------API For Order Table -----------------

    //// ------------------API For OrderDetails Table -----------------

    [WebMethod]
    public int insertOrderDetails(string Product_id, int Order_id, int Quantity, float Price, float Discount)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO [FloristDB].[dbo].[Order_detail] (Product_id , Order_id , Quantity, Price, Discount ) " +
                " VALUES( N'" + Product_id + "' , " +
                + Order_id + " , " +
                + Quantity + " , " +
                + Price + " , " +
                +Discount + ")";
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }
    //// ------------------API For Banking Table -----------------

    [WebMethod]
    public int insertBanking(string Banking_name, string Number, int Customer_id, int Order_id, float Total)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO [FloristDB].[dbo].[Banking] (Banking_name , Number , Customer_id, Order_id, Total ) " +
                " VALUES( N'" + Banking_name + "' , " +
                " N'" + Number + "', " +
                +Customer_id + " , " +
                +Order_id + " , " +
                +Total + ")";
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = cnn.CreateCommand();
            cmd.Connection = cnn;
            cmd.CommandText = sql.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (System.Exception)
        {

            return 0;
            throw;
        }
        return 0;

    }

    //// ------------------API For Banking Table -----------------


}
