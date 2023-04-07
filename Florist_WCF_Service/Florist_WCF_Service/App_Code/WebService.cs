using System;
using System.Activities.Expressions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet GetAllMessageByID(int id)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Message_id, Message_content, Category FROM [FloristDB].[dbo].[Message] Where Message_id = " + id;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet GetAllMessages()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Message_id, Message_content, Category, Customer_id FROM [FloristDB].[dbo].[Message] Where Customer_id IS NULL";
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
    public int updateMessageByID(int Message_id, string Message_content,  string Category)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE [FloristDB].[dbo].[Message] SET ");
            sql.Append(" Message_content = N'" + Message_content + "', ");
            sql.Append(" Category = N'" + Category + "' ");
            sql.Append(" WHERE  Message_id  =  " + Message_id);

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
    public int createMessageAdmin(string Category, string Message_content)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO [FloristDB].[dbo].[Message] (Category ,Message_content) " +
                " VALUES( N'" + Category + "', " +
                "'"+ Message_content + "')";

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
    public int deleteMessageAdmin(int Message_id)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "DELETE  FROM  [FloristDB].[dbo].[Message] WHERE Message_id = " + Message_id;
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

    [WebMethod]
    public DataSet GetAllProducts()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string queryCategory = "";
        string sql = "SELECT Product.Product_id, Product.Product_name, Product.Description," +
            " Product.Image, Product.Price, Product.Discount,Product.Category_id, Category.Category_name," +
            " Product.Supplier_id, Supplier.Supplier_name, Product.Update_by, Account.User_name, Product.Sold," +
            " Product.Inventory, Product.Update_at FROM     Product" +
            " LEFT OUTER JOIN    Category ON Product.Category_id = Category.Category_id" +
            " LEFT OUTER JOIN  Account ON Product.Update_by = Account.Account_id LEFT OUTER JOIN  Supplier ON Product.Supplier_id = Supplier.Supplier_id  ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet GetAllProductsID(string ID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string queryCategory = "";
        string sql = "SELECT Product.Product_id, Product.Product_name, Product.Description," +
            " Product.Image, Product.Price, Product.Discount,Product.Category_id, Category.Category_name," +
            " Product.Supplier_id, Supplier.Supplier_name, Product.Update_by, Account.User_name, Product.Sold," +
            " Product.Inventory, Product.Update_at FROM     Product" +
            " LEFT OUTER JOIN    Category ON Product.Category_id = Category.Category_id" +
            " LEFT OUTER JOIN  Account ON Product.Update_by = Account.Account_id LEFT OUTER JOIN  Supplier ON Product.Supplier_id = Supplier.Supplier_id  " +
            " Where  Product.Product_id  = N'" + ID + "'";

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

    [WebMethod]
    public int checkExitsProduct(string productID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT COUNT(*) as count FROM  Product " +
            " Where  Product.Product_id = '" + productID + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }
    [WebMethod]
    public int updateProductByID(string Product_id, string Product_name, string Description, string Image,
        float Price, float Discount, int Category_id, int Supplier_id, int Update_by, int Inventory, int Sold)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "UPDATE [FloristDB].[dbo].[Product] SET " +
                " Product_name = N'" + Product_name + "', " +
                " Description = N'" + Description + "', " +
                " Image = N'" + Image + "', " +
                " Price = " + Price + "," +
                " Discount = " + Discount + "," +
                " Category_id = " + Category_id + "," +
                " Supplier_id = " + Supplier_id + "," +
                " Update_by = " + Update_by + "," +
                " Sold = " + Sold + "," +
                " Inventory = " + Inventory + "," +
                " Update_at = GETDATE() " +
                " WHERE Product.Product_id = '" + Product_id + "'";

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
    public int deleteProductsByID(string id)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            StringBuilder sql = new StringBuilder();
            sql.Append(" DELETE FROM [Product] ");
            sql.Append(" WHERE  Product_id  =  '" + id + "'");

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
    public int insertGetProductByID(string Product_id, string Product_name, string Description, string Image,
        float Price, float Discount, int Category_id, int Supplier_id, int Update_by, int Inventory)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO Product (Product_id , Product_name , [Description] ,[Image] ,Price " +
                " ,Discount ,Category_id ,Supplier_id ,Update_by ,Sold ,Inventory ,Update_at) " +
                " VALUES( N'" + Product_id + "', " +
                " N'" + Product_name + "', " +
                " N'" + Description + "', " +
                " N'" + Image + "', " +
                " N'" + Price + "', " +
                " N'" + Discount + "', " +
                Category_id  + ", " +
                Supplier_id + ", " +
                Update_by + ", " +
                 0 + ", " +
                Inventory + ", " 
                + "GETDATE()" + ")";

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

    [WebMethod]
    public int checkRegistrationCustomer(string Phone)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Count(*) as count FROM  [FloristDB].[dbo].[Customer] WHERE Phone = N'" + Phone +  "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }


    ////3. Api Register Customer Table
    [WebMethod]
    public int registrationCustomer(string FirstName, string LastName, string Phone, string Password)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO [FloristDB].[dbo].[Customer] (First_name, Last_name, Phone, Password) VALUES (N'" + FirstName + "',N'" + LastName + "',N'" + Phone + "',N'" + Password + "')";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (Exception)
        {
            return 0;
            throw;
        }
        
    }

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

    [WebMethod]
    public DataSet GetAllEvaluteByID(int id)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Evalute.Evalute_id, Evalute.Evalute_content, Evalute.Rate, Evalute.Create_at, " +
            " Product.Product_id, Customer.Customer_id, Customer.First_name, Customer.Last_name, Product.Product_name " +
            " FROM     Evalute " +
            " LEFT OUTER JOIN    Customer ON Evalute.Create_by = Customer.Customer_id " +
            " LEFT OUTER JOIN    Product ON Evalute.Product_id = Product.Product_id " +
            " WHERE  Evalute_id = " + id;
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
    public DataSet GetAllOrder()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = " SELECT Order_id, Status, Shipper_id, Flower_recipient_id, " 
                  + " Create_at, Create_by, CONCAT(Customer.First_name, ' ', Customer.Last_name) as fullname, Message_id, Received_date, Received_time, Total " 
                  + " FROM[FloristDB].[dbo].[Order] " 
                  + " Inner Join[FloristDB].[dbo].[Customer] on[Customer].Customer_id = [Order].Create_by " +
                  " Where [Order].Received_date IS NOT NULL " +
                  " ORDER BY Received_date DESC ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }
    [WebMethod]
    public DataSet GetOrderByID(int ID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = " SELECT [Order].Order_id, [Order].Status, [Order].Shipper_id, Account.Employee_name,  "
                  + " [Order].Flower_recipient_id,  Flower_recipient.Name, Flower_recipient.Address,  "
                  + " Flower_recipient.Phone ,  [Order].Create_at, [Order].Create_by,  " +
                  " CONCAT(Customer.First_name, ' ', Customer.Last_name) as fullname,   "
                  + " [Order].Message_id,Message.Message_content,  [Order].Received_date,  "
                  + " [Order].Received_time, [Order].Total  FROM[FloristDB].[dbo].[Order]   "
                  + " LEFT Join[FloristDB].[dbo].[Customer] on[Customer].Customer_id = [Order].Create_by "
                  + " LEFT Join[FloristDB].[dbo].[Account] on [Account].Account_id = [Order].Shipper_id "
                  + " LEFT Join[FloristDB].[dbo].[Flower_recipient] on [Flower_recipient].Flower_recipient_id = [Order].Flower_recipient_id "
                  + " LEFT Join[FloristDB].[dbo].[Message] on [Message].Message_id = [Order].Message_id " +
                  " Where [Order].Received_date IS NOT NULL AND [Order].Order_id = " + ID + 
                  " ORDER BY Received_date DESC ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public int insertOrder(int customerID, int Flower_recipient_id, float Total, int Message_id, string Received_date, string Received_time)
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
    public DataSet GetOrderDetailByID(int orderID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = " SELECT Order_detail.Order_detail_id, Order_detail.Quantity, Order_detail.Price, Order_detail.Discount,Product.Product_id,Order_detail.Order_id, Product.Product_name "
                  + " FROM[FloristDB].[dbo].[Order_detail] "
                  + " Inner Join[FloristDB].[dbo].[Order] on [Order].Order_id = [Order_detail].Order_id " +
                  " Inner Join[FloristDB].[dbo].[Product] on [Product].Product_id = [Order_detail].Product_id " +
                  " WHERE Order_detail.Order_id =  " + orderID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet GetOrderDetailByDetailsID(int orderDetailsID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = " SELECT Order_detail.Order_detail_id, Order_detail.Quantity, Order_detail.Price, Order_detail.Discount,Product.Product_id,Order_detail.Order_id, Product.Product_name "
                  + " FROM[FloristDB].[dbo].[Order_detail] "
                  + " Inner Join[FloristDB].[dbo].[Order] on [Order].Order_id = [Order_detail].Order_id " +
                  " Inner Join[FloristDB].[dbo].[Product] on [Product].Product_id = [Order_detail].Product_id " +
                  " WHERE Order_detail.Order_detail_id =  " + orderDetailsID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


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


    //// ------------------API For Account Table -----------------
    [WebMethod]
    public DataSet getAllAdmin()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account.* , [Group].Group_name  FROM  [FloristDB].[dbo].[Account] " +
            " INNER JOIN  [FloristDB].[dbo].[Group] ON  [Group].Group_id =  [Account].Group_id ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet getAllAdminByID(int id)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account.* , [Group].Group_name  FROM  [FloristDB].[dbo].[Account] " +
            " INNER JOIN  [FloristDB].[dbo].[Group] ON  [Group].Group_id =  [Account].Group_id " +
            " WHERE Account.Account_id = " + id;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public int checkLoginAdmin(string UserName, string Password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Count(*) as count FROM  [FloristDB].[dbo].[Account] WHERE User_name = N'" + UserName + "'AND Password = N'" + Password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return int.Parse(ds.Tables[0].Rows[0]["count"].ToString());
    }
    // 1. Api Login Account Table
    [WebMethod]
    public DataSet LoginAccountAdmin(string UserName, string Password)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT * FROM  [FloristDB].[dbo].[Account] " +
            "WHERE User_name = N'" + UserName + "'AND Password = N'" + Password + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet getListUrl(int groupID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT  [Function].Base_url FROM [FloristDB].[dbo].[Group_function] " +
            " INNER JOIN  [FloristDB].[dbo].[Function] ON  [Group_function].Function_id =  [Function].Function_id " +
            " WHERE [Group_function].[Group_id] = " + groupID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }
    [WebMethod]
    public int insertAccountAdmin(string User_name, string Password, int Group_id, string Employee_name, string Phone)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "INSERT INTO [FloristDB].[dbo].[Account] (User_name , Password , Group_id, Employee_name, Phone ) " +
                " VALUES( N'" + User_name + "' , " +
                " N'" + Password + "', " +
                + Group_id + " , " +
                " N'" + Password + "', " +      
                "N'"  + Phone + "')";
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
    public int updateAdminByID(int Account_id, string User_name, string Password, int Group_id, string Employee_name, string Phone)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE [FloristDB].[dbo].[Account] SET ");
            sql.Append(" User_name = N'" + User_name + "', ");
            sql.Append(" Password = N'" + Password + "', ");
            sql.Append(" Group_id = " + Group_id + " , ");
            sql.Append(" Employee_name = N'" + Employee_name + "', ");
            sql.Append(" Phone  =  N'" + Phone + "' ");
            sql.Append(" WHERE  Account_id  =  " + Account_id);

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
    public int deleteAdminByAdmin(int id)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            string sql = "DELETE  FROM  [FloristDB].[dbo].[Account] WHERE Account_id = " + id;
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



    //// ------------------API For Account Table -----------------
    ///
    //// ------------------API For Group Table -----------------
    [WebMethod]
    public DataSet getAllGroup()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT * FROM [FloristDB].[dbo].[Group] ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //// ------------------API For Group Table -----------------
    ///
    //// ------------------API For Supplier Table -----------------

    [WebMethod]
    public DataSet GetAllSupplier()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Supplier_id,Supplier_name, Address, Phone FROM Supplier", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API Get All Supplier By Supplier_name
    [WebMethod]
    public DataSet GetAllSupplierByID(int Supplier_id)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Supplier_id,Supplier_name, Address, Phone FROM Supplier WHERE Supplier_id = " + Supplier_id, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    //3.API Insert Supplier
    [WebMethod]
    public int InsertSupplier(string Supplier_name, string Address, string Phone)
    {
        string pattern = "'";
        string replacement = "''";
        Supplier_name = Regex.Replace(Supplier_name, pattern, replacement);
        Address = Regex.Replace(Address, pattern, replacement);
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Supplier (Supplier_name, Address, Phone) VALUES (N'" + Supplier_name + "', N'" + Address + "', '" + Phone + "')";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;

        }
        catch (Exception)
        {
            return (int)0;
        }
    }
    //4. API Update Supplier
    [WebMethod]
    public int UpdateSupplier(int SupplierID, string SupplierName, string Address, string Phone)
    {


        string pattern = "'";
        string replacement = "''";
        string Supplier_name = Regex.Replace(SupplierName, pattern, replacement);
        string Address_ = Regex.Replace(Address, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Supplier SET Supplier_name = N'" + Supplier_name + "', Address = N'" + Address_ + "', Phone = '" + Phone + "' WHERE Supplier_id = " + SupplierID;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;
        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    //5.API Delete Supplier
    [WebMethod]
    public int DeleteSupplier(int SupplierID)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "DELETE FROM Supplier WHERE Supplier_id = " + SupplierID;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;
        }
        catch (Exception)
        {
            return (int)0;
        }
    }


    //// ------------------API For Supplier Table -----------------


    // --------------------------API For Category Table -------------------------------
    //1.Api get All Category
    [WebMethod]
    public DataSet GetAllCategory()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Category_id, Category_name FROM [FloristDB].[dbo].[Category] ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //2. Api get Category by Category_id
    [WebMethod]
    public DataSet GetCategoryID(int CategoryID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Category_id, Category_name FROM [FloristDB].[dbo].[Category]  Where  Category.Category_id = " + CategoryID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //3.API Insert Category
    [WebMethod]
    public int InsertCategory(string CategoryName)
    {
        string pattern = "'";
        string replacement = "''";
        string Category_name = Regex.Replace(CategoryName, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Category (Category_name) VALUES (N'" + Category_name + "')";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;

        }
        catch (Exception)
        {
            return (int)0;
        }
    }
    ////4. API Update Category
    [WebMethod]
    public int UpdateCategory(int CategoryID, string CategoryName)
    {


        string pattern = "'";
        string replacement = "''";
        string Category_name = Regex.Replace(CategoryName, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Category SET Category_name = N'" + Category_name + "' WHERE Category_id = " + CategoryID;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;
        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    //5.API Delete Category
    [WebMethod]
    public int DeleteCategory(int CategoryID)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "DELETE FROM Category WHERE Category_id = " + CategoryID;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;
        }
        catch (Exception)
        {
            return (int)0;
        }
    }


    //// ------------------API For Authentication -----------------
    ///
    //------------------API For Evalute Table -----------------------

    // 6. API Delete a Evalute
    [WebMethod]
    public int DeleteEvalute(int EvaluteID)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "DELETE FROM Evalute WHERE Evalute_id = " + EvaluteID;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;
        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    //------------------API For Evalute Table -----------------------

    //------------------API For Customer Table -----------------------


    [WebMethod]
    public DataSet GetAllCustomer()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Customer.*   FROM  [FloristDB].[dbo].[Customer] ";

        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    [WebMethod]
    public DataSet GetCustomerByID(int id)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = " SELECT * FROM  [FloristDB].[dbo].[Customer]  WHERE Customer_id = " + id;

        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    [WebMethod]
    public int insertCustomer(string FirstName, string LastName, string Phone, string Password)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO [FloristDB].[dbo].[Customer] (First_name, Last_name, Phone, Password, Address) VALUES (N'" + FirstName + "',N'" + LastName + "',N'" + Phone + "',N'" + Password + "')";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return 1;
        }
        catch (Exception)
        {
            return 0;
            throw;
        }
    }


    [WebMethod]
    public int deleteCustomer(int CustomerID)
    {
        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "DELETE FROM [FloristDB].[dbo].[Customer] WHERE Customer_id = " + CustomerID;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            return (int)1;
        }
        catch (Exception)
        {
            return (int)0;
        }
    }











    //------------------API For Customer Table -----------------------


}
