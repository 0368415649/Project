using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WebServiceShopping
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebServiceShopping : System.Web.Services.WebService
{
    string connstr = ConfigurationManager.ConnectionStrings["florist"].ConnectionString;
    public WebServiceShopping()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    // ------------  API For Message Table -----------

    //1. Api get all message
    [WebMethod]
    public DataSet GetAllMessage()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Message_content,Category FROM Message", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. Api get Message_content by Category
    [WebMethod]
    public DataSet GetMessageByCategory(string CategoryName)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Message_content FROM Message WHERE Category = '" + CategoryName + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql , cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // -----------------API For Product Table -------------

    //1. Api get All Product (
    [WebMethod]
    public DataSet GetAllProduct()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Product.Product_id, Product.Product_name, Product.Description, Product.Image, Product.Price, Product.Discount AS Discount_product, Product.Sold, Product.Update_by, Product.Inventory, Product.Update_at,  Event.Event_name, Event.Discount AS Discount_event, Supplier.Supplier_name, Account.Account_id, Category.Category_name, Account.Employee_name FROM     Product LEFT OUTER JOIN Supplier ON Product.Supplier_id = Supplier.Supplier_id LEFT OUTER JOIN Event ON Product.Event_id = Event.Event_id LEFT OUTER JOIN Category ON Product.Category_id = Category.Category_id LEFT OUTER JOIN Account ON Product.Update_by = Account.Account_id";
        
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // ------------------API For Authentication -----------------

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
        string sql = "SELECT Customer_id, First_name, Last_name, Password, Sex, Birth_day, Phone, Address FROM  Customer WHERE Phone = N'" + Phone + "'AND Password = N'" + Password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        if(ds.Tables[0].Rows.Count > 0)
        {
            return ds;
        }
        else
        {
            return 0;
        }
    }

    //3. Api Register Customer Table
    [WebMethod]
    public DataSet LoginCustomer(string FirstName,string LastName, string Phone, string Password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        cnn.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        string sql = "INSERT INTO Customer (First_name, Last_name, Phone, Password) VALUES (N'" + FirstName + "',N'" + LastName + "',N'" + Phone + "',N'" + Password +     "')";
        cmd.CommandText = sql;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        return 1;
    }


    //------------------API For Evalute Table -----------------------

    //1.0 API get All Evalute Table
    [WebMethod]
    public DataSet GetAllEvalute()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("Evalute_id,Create_by,Product_id,Evalute_content,Rate,Create_at FROM Evalute", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //1.Api get Evalute by Product
    [WebMethod]
    public DataSet GetMessageByCategory(string ProductID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute_id,Create_by,Product_id,Evalute_content,Rate,Create_at FROM Evalute WHERE Product_id = N'" + ProductID + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API get Evalute by Rate
    [WebMethod]
    public DataSet GetMessageByCategory(string Rate)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute_id,Create_by,Product_id,Evalute_content,Rate,Create_at FROM Evalute WHERE Rate = N'" + Rate + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //-------------------API FOr 



}