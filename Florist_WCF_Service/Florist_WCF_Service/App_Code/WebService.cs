using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // -----------------API For Product Table -------------

    //1. Api get All Product by search, page, sort 
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

    //1. Api get All Product by search, page, sort 
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

    //1. Api get All Product by category(
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

    //// ------------------API For Authentication -----------------

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


    //------------------API For Evalute Table -----------------------

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
    public DataSet GetEvaluteByProduct(string ProductID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute.Evalute_id, Evalute.Evalute_content, Evalute.Rate, Evalute.Create_at, Product.Product_id, Customer.Customer_id, Customer.First_name, Customer.Last_name, Product.Product_name FROM     Evalute LEFT OUTER JOIN    Customer ON Evalute.Create_by = Customer.Customer_id LEFT OUTER JOIN    Product ON Evalute.Product_id = Product.Product_id WHERE Evalute.Product_id = N'" + ProductID + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API get Evalute by Rate
    [WebMethod]
    public DataSet GetEvaluteByRate(string Rate)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute.Evalute_id, Evalute.Evalute_content, Evalute.Rate, Evalute.Create_at, Product.Product_id, Customer.Customer_id, Customer.First_name, Customer.Last_name, Product.Product_name FROM     Evalute LEFT OUTER JOIN    Customer ON Evalute.Create_by = Customer.Customer_id LEFT OUTER JOIN    Product ON Evalute.Product_id = Product.Product_id WHERE Evalute.Rate = " + Rate;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

}
