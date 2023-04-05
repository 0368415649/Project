using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Xml.Linq;
using BC = BCrypt.Net.BCrypt;

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

    //3. Api Insert Message
    [WebMethod]
    public int InsertMessage(string MessageContent, string Category, int CustomerID)
    {
        string pattern = "'";
        string replacement = "''";
        string Message_content = Regex.Replace(MessageContent, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Message (Message_content, Category, Customer_id) VALUES (N'" + Message_content+ "', N'"+ Category + "'," + CustomerID + ")";
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
    //4. Api Update Message
    [WebMethod]
    public int UpdateMessage(int MessageID, string MessageContent, string Category, int CustomerID)
    {


        string pattern = "'";
        string replacement = "''";
        string Message_content = Regex.Replace(MessageContent, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Message SET Message_content = N'" + Message_content + "', Category = N'"+ Category + "', Customer_id = " + CustomerID + " WHERE Message_id = " + MessageID;
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

    //5. Api Delete Message
    [WebMethod]
    public int DeleteMessage(int MessageID)
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
            string sql = "DELETE FROM Message WHERE Message_id = " + MessageID;
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

    // -----------------API For Product Table -------------

    //1. Api Show All Product:
    //Product Table: All Column,
    //Account Table: Employee_name column
    //Supplier Table: Supplier_name column
    //Category Table: Category_name column
    [WebMethod]
    public DataSet ShowAllProduct()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Product.Product_id, Product.Product_name, Product.Description, Product.Image, Product.Price, Product.Discount, Product.Sold, Product.Inventory, Product.Update_at, Product.Category_id, Product.Supplier_id, Product.Update_by, Account.Employee_name, Category.Category_name, Supplier.Supplier_name FROM Product INNER JOIN  Account ON Product.Update_by = Account.Account_id INNER JOIN Category ON Product.Category_id = Category.Category_id INNER JOIN Supplier ON Product.Supplier_id = Supplier.Supplier_id", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
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

    //3. Api get Count Product by Name and Category_id
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

    //4. Api get Product By Category_id
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

    //5. Api Get Product By Search: Search by Product_id, Product_name, Description 
    [WebMethod]
    public DataSet GetProductBySearch(string Search)
    {
        string pattern = "'";
        string replacement = "''";
        string Search_ = Regex.Replace(Search, pattern, replacement);

        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Product.Product_id, Product.Product_name, Product.Description, Product.Image, Product.Price, Product.Discount, Product.Sold, Product.Inventory, Product.Update_at, Product.Category_id, Product.Supplier_id, Product.Update_by, Account.Employee_name, Category.Category_name, Supplier.Supplier_name FROM Product INNER JOIN  Account ON Product.Update_by = Account.Account_id INNER JOIN Category ON Product.Category_id = Category.Category_id INNER JOIN Supplier ON Product.Supplier_id = Supplier.Supplier_id WHERE Product.Product_id LIKE N'%" + Search_ + "%' OR Product.Product_name LIKE N'%"+ Search_ + "%' OR Product.Description LIKE N'%"+ Search_ + "%'", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    //6.Api Insert new Product
    [WebMethod]
    public int InsertProduct(string ProductID, string Product_name, string InputDescription, string Image, float Price, float Discount, int Category_id, int Supplier_id, int Sold, int Inventory, byte Picture)
    {
        DateTime CurrentDate = DateTime.Now;
        int day = CurrentDate.Day;
        string dayString = day > 9 ? day.ToString() : '0' + day.ToString();
        int month = CurrentDate.Month;
        string monthString = month > 9 ? month.ToString() : '0' + month.ToString();
        int year = CurrentDate.Year;
        string Update_at = year.ToString() + '-'+ monthString + '-' + dayString;

        string pattern = "'";
        string replacement = "''";
        string Description = Regex.Replace(InputDescription, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Product (Product_id, Product_name, Description,Image,Price,Discount,Category_id,Supplier_id,Update_by,Sold,Inventory,Update_at) VALUES (N'" + ProductID + "', N'" + Product_name + "' ,N'" + Description+ "',N'"+ Image + "',"+ Price + "," + Discount+ ", "+ Category_id+ ", "+ Supplier_id+ ", "+ 1+ ", "+ Sold+ ", "+ Inventory+ ", '"+ Update_at + "')";
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
    //7. Api Update a Product 
    [WebMethod]
    public int UpdateProduct(string ProductID, string Product_name, string InputDescription,string Image, float Price, float Discount,int Category_id, int Supplier_id, int Sold, int Inventory)
    {
        DateTime CurrentDate = DateTime.Now;
        int day = CurrentDate.Day;
        string dayString = day > 9 ? day.ToString() : '0' + day.ToString();
        int month = CurrentDate.Month;
        string monthString = month > 9 ? month.ToString() : '0' + month.ToString();
        int year = CurrentDate.Year;
        string Update_at = year.ToString() + '-' + monthString + '-' + dayString;

        string pattern = "'";
        string replacement = "''";
        string Description = Regex.Replace(InputDescription, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Product SET Product_id = N'" + ProductID + "', Product_name = N'" + Product_name + "', Description = N'" + Description + "',Image = N'" + Image + "', Price =  " + Price + ",Discount = " + Discount+ ", Category_id = " + Category_id + ", Supplier_id = "+ Supplier_id + ",Update_by =  "+ 1 + ", Sold = "+ Sold + ", Inventory = "+ Inventory + ", Update_at = '" + Update_at + "' WHERE Product_id = N'" + ProductID + "'";
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
    //8. Api Delete a Product
    [WebMethod]
    public int DeleteProduct(string ProductID)
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
            string sql = "DELETE FROM Product WHERE Product_id = N'" + ProductID + "'";
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
    public string GetCategoryName(int CategoryID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Category_name FROM [FloristDB].[dbo].[Category]  Where  Category.Category_id = " + CategoryID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds.Tables[0].Rows[0]["Category_name"].ToString();
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
    //4. API Update Category
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
            string sql = "UPDATE Category SET Category_name = N'" + Category_name + "' WHERE Category_id = " + CategoryID ;
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

    
    //1. API Check Login Customer by Phone and Password
    [WebMethod]
    public int CheckLoginCustomer(string Phone, string Password)
    {
        string pattern = "'";
        string replacement = "''";
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);


        SqlConnection cnn = new SqlConnection(connstr);

        string selectPhone = "SELECT Count(*) as count FROM  [FloristDB].[dbo].[Customer] WHERE Phone = N'" + Phone + "'";
        SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
        DataSet dsPhone = new DataSet();
        adapterPhone.Fill(dsPhone);
        int checkPhone =  int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

        if (checkPhone > 0)
        {
        string selectPass = "SELECT Password FROM  [FloristDB].[dbo].[Customer] WHERE Phone = N'" + Phone + "'";
        SqlDataAdapter adapterPass = new SqlDataAdapter(selectPass, cnn);
        DataSet dsPass = new DataSet();
        adapterPass.Fill(dsPass);

        string getPassbyPhone = dsPass.Tables[0].Rows[0]["Password"].ToString();

        bool verified = BCrypt.Net.BCrypt.Verify(Password, getPassbyPhone);
            if (verified)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    // 2. Api Get Customer By Phone, Used when customer login successfully
    [WebMethod]
    public DataSet GetCustomerByPhone(string Phone)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Customer_id, First_name, Last_name, Sex, Birth_day, Phone, Address FROM  [FloristDB].[dbo].[Customer] WHERE Phone = N'" + Phone + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        return ds;
    }

    //3. API Check Login Account by Phone and Password
    [WebMethod]
    public int CheckLoginAccount(string Phone, string Password)
    {
        string pattern = "'";
        string replacement = "''";
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);


        SqlConnection cnn = new SqlConnection(connstr);

        string selectPhone = "SELECT Count(*) as count FROM  [FloristDB].[dbo].[Account] WHERE Phone = N'" + Phone + "'";
        SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
        DataSet dsPhone = new DataSet();
        adapterPhone.Fill(dsPhone);
        int checkPhone = int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

        if (checkPhone > 0)
        {
            string selectPass = "SELECT Password FROM  [FloristDB].[dbo].[Account] WHERE Phone = N'" + Phone + "'";
            SqlDataAdapter adapterPass = new SqlDataAdapter(selectPass, cnn);
            DataSet dsPass = new DataSet();
            adapterPass.Fill(dsPass);

            string getPassbyPhone = dsPass.Tables[0].Rows[0]["Password"].ToString();

            bool verified = BCrypt.Net.BCrypt.Verify(Password, getPassbyPhone);
            if (verified)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    // 4. Api Get Account By Phone, Used when Admin/employee login successfully
    [WebMethod]
    public DataSet GetAccountByPhone(string Phone)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT  Account.User_name,  Account.Account_id,  Account.Employee_name,  Account.Phone,  [Group].Group_name,  [Function].Function_name,  [Function].Base_url FROM      Account LEFT OUTER JOIN  [Group] ON  Account.Group_id =  [Group].Group_id FULL OUTER JOIN  Group_function FULL OUTER JOIN  [Function] ON  Group_function.Function_id =  [Function].Function_id ON  [Group].Group_id =  Group_function.Group_id WHERE Account.Phone = N'" + Phone + "'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        return ds;
    }

    //5. Api Register Customer Table
    [WebMethod]
    public int RegisterCustomer(string FirstName, string LastName, string Phone, string Password)
    {
        string pattern = "'";
        string replacement = "''";
        string First_name = Regex.Replace(FirstName, pattern, replacement);
        string Last_name = Regex.Replace(LastName, pattern, replacement);
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);

            string selectPhone = "SELECT Count(*) as count FROM  Customer WHERE Phone = N'" + Phone + "'";
            SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
            DataSet dsPhone = new DataSet();
            adapterPhone.Fill(dsPhone);
            int checkPhone = int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

            if(checkPhone == 0)
            {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                string sql = "INSERT INTO Customer (First_name, Last_name, Phone, Password) VALUES (N'" + First_name + "', N'"+ Last_name+ "', N'"+ Phone + "', N'"+ PasswordHash + "')";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return (int)1;
            }
            else
            {
                return (int)0;
            }
        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    //6. Api Register Account Table
    [WebMethod]
    public int RegisterAccount(string UserName, string Password, int GroupID,string EmployeeName, string Phone)
    {
     
        string pattern = "'";
        string replacement = "''";
        string User_name = Regex.Replace(UserName, pattern, replacement);
        string Employee_name = Regex.Replace(EmployeeName, pattern, replacement);
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);

            string selectPhone = "SELECT Count(*) as count FROM  Account WHERE Phone = N'" + Phone + "'";
            SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
            DataSet dsPhone = new DataSet();
            adapterPhone.Fill(dsPhone);
            int checkPhone = int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

            if(checkPhone == 0) {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                string sql = "INSERT INTO Account (User_name, Password, Group_id, Employee_name, Phone) VALUES (N'" + User_name + "', N'" + PasswordHash + "', " + GroupID + ", N'" + EmployeeName + "', N'"+ Phone +"')";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return (int)1;
            }
            else
            {
                return (int)0;
            }
        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    // ---------------------- API For Account Table --------------------

    //1.API Get All Account
    [WebMethod]
    public DataSet GetAllAccount()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account_id, User_name, Group_id, Employee_name, Phone FROM Account";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //2.Api Get Account By ID
    [WebMethod]
    public DataSet GetAccountByID(int AccountID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account_id, User_name,  Group_id, Employee_name, Phone FROM Account WHERE Account_id = "+ AccountID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //3.Api Get Account By Search
    [WebMethod]
    public DataSet GetAccountBySearch(string Search)
    {
        string pattern = "'";
        string replacement = "''";
        string Search_ = Regex.Replace(Search, pattern, replacement);

        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account_id, User_name, Group_id, Employee_name, Phone FROM Account WHERE User_name LIKE N'%" + Search_ + "%' OR Employee_name LIKE N'%" + Search_ + "%'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }



    //4. API get Url getUrlByAccountID(int account_id) By AccountID
    [WebMethod]
    public DataSet GetUrlByAccountID(int AccountID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account.Account_id, Account.User_name, Account.Password, Account.Group_id, Account.Employee_name, Account.Phone, [Function].Function_name, [Function].Base_url FROM     Account RIGHT OUTER JOIN  [Function] RIGHT OUTER JOIN  Account_function ON [Function].Function_id = Account_function.Function_id ON Account.Account_id = Account_function.Account_id WHERE Account.Account_id = " + AccountID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //5. API InsertAccount  is  RegisterAccount
    //6. API UpdateAccount
    [WebMethod]
    public int UpdateAccount(int AccountID, string UserName, string Password, int GroupID, string EmployeeName, string Phone)
    {
        string pattern = "'";
        string replacement = "''";
        string User_name = Regex.Replace(UserName, pattern, replacement);
        string Employee_name = Regex.Replace(EmployeeName, pattern, replacement);
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);

            string selectPhone = "SELECT Count(*) as count FROM  Account WHERE Phone = N'" + Phone + "'";
            SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
            DataSet dsPhone = new DataSet();
            adapterPhone.Fill(dsPhone);
            int checkPhone = int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

            string sqlGetPhone = "SELECT Phone FROM  Account WHERE Account_id = " + AccountID;
            SqlDataAdapter adapterGetPhone = new SqlDataAdapter(sqlGetPhone, cnn);
            DataSet dsGetPhone = new DataSet();
            adapterGetPhone.Fill(dsGetPhone);
            string getPhone = dsGetPhone.Tables[0].Rows[0]["Phone"].ToString();

            if (checkPhone == 0 || getPhone == Phone) {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                string sql = "UPDATE Account SET User_name = N'" + User_name + "', Password = N'" + PasswordHash + "', Group_id = " + GroupID + ", Employee_name = N'" + Employee_name + "', Phone = N'" + Phone + "' WHERE Account_id = " + AccountID;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return (int)1;
            }
            else
            {
                return (int)0;
            }
            
        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    //7. API DeleteAccount
    [WebMethod]
    public int DeleteAccount(int AccountID)
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
            string sql = "DELETE FROM Account WHERE Account_id = " + AccountID;
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

    //------------------------API For Account_function Table ----------------------------

    //1.Api Get All AccountFunction
    [WebMethod]
    public DataSet GetAllAccountFunction()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Account_id, Function_id FROM Account_function";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //2. Api Insert Account Function
    [WebMethod]
    public int InsertAccountFunction(int AccountID, int FunctionID)
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
            string sql = "INSERT INTO Account_function (Account_id, Function_id) VALUES (" + AccountID + ","+ FunctionID + ")";
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


    // ---------------------- API For Customer Table --------------------

    //1.API Get All Customer
    [WebMethod]
    public DataSet GetAllCustomer()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Customer_id, First_name, Last_name, Password, Sex, Birth_day, Phone, Address FROM Customer";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    //3. API InsertCustomer  All Column
    [WebMethod]
    public int InsertCustomerAllColumn(string FirstName, string LastName, string Password, string Sex, string Phone, string Address)
    {
        string pattern = "'";
        string replacement = "''";
        string First_name = Regex.Replace(FirstName, pattern, replacement);
        string Last_name = Regex.Replace(LastName, pattern, replacement);
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);

            string selectPhone = "SELECT Count(*) as count FROM  Customer WHERE Phone = N'" + Phone + "'";
            SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
            DataSet dsPhone = new DataSet();
            adapterPhone.Fill(dsPhone);
            int checkPhone = int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

            if (checkPhone == 0)
            {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                string sql = "INSERT INTO Customer (First_name, Last_name, Password, Sex, Phone, Address) VALUES (N'" + First_name + "', N'" + Last_name + "', N'" + PasswordHash + "', N'" + Sex + "', N'"+ Phone+ "', N'"+ Address +"')";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return (int)1;
            }
            else
            {
                return (int)0;
            }
        }
        catch (Exception)
        {
            return (int)0;
        }
    }


    //4. API UpdateCustomer
    [WebMethod]
    public int UpdateCustomer(int CustomerID, string FirstName, string LastName, string Password, string Sex, string Phone, string Address)
    {
        string pattern = "'";
        string replacement = "''";
        string First_name = Regex.Replace(FirstName, pattern, replacement);
        string Last_name = Regex.Replace(LastName, pattern, replacement);
        string Address_ = Regex.Replace(Address, pattern, replacement);
        string Pass = Regex.Replace(Password, pattern, replacement);

        string PasswordHash = BC.HashPassword(Pass);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);

            string selectPhone = "SELECT Count(*) as count FROM  Customer WHERE Phone = N'" + Phone + "'";
            SqlDataAdapter adapterPhone = new SqlDataAdapter(selectPhone, cnn);
            DataSet dsPhone = new DataSet();
            adapterPhone.Fill(dsPhone);
            int checkPhone = int.Parse(dsPhone.Tables[0].Rows[0]["count"].ToString());

            string sqlGetPhone = "SELECT Phone FROM  Customer WHERE Customer_id = " + CustomerID;
            SqlDataAdapter adapterGetPhone = new SqlDataAdapter(sqlGetPhone, cnn);
            DataSet dsGetPhone = new DataSet();
            adapterGetPhone.Fill(dsGetPhone);
            string getPhone =dsGetPhone.Tables[0].Rows[0]["Phone"].ToString();

            if (checkPhone == 0 || getPhone == Phone)
            {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                string sql = "UPDATE Customer SET First_name = N'" + First_name + "', Last_name = N'" + Last_name + "', Password = N'" + PasswordHash + "', Sex = N'" + Sex + "', Phone = N'" + Phone + "', Address = N'"+ Address_ +"' WHERE Customer_id = " + CustomerID;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return (int)1;
            }
            else
            {
                return (int)0;
            }

        }
        catch (Exception)
        {
            return (int)0;
        }
    }

    //5. API DeleteAccount
    [WebMethod]
    public int DeleteCustomer(int CustomerID)
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
            string sql = "DELETE FROM Customer WHERE Customer_id = " + CustomerID;
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

    //1 API get All Evalute Table
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

    //2.Api get Evalute by Product
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

    // 3. API get Evalute by Rate
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

    // 4. API Insert a Evalute
    [WebMethod]
    public int InsertEvalute(int CreateBy, string ProductID, string EvaluteContent, int Rate)
    {
        DateTime CurrentDate = DateTime.Now;

        string pattern = "'";
        string replacement = "''";
        string Evalute_Content = Regex.Replace(EvaluteContent, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Evalute (Create_by, Product_id, Evalute_content, Rate, Create_at) VALUES (" + CreateBy + ", N'" + ProductID + "', N'"+ Evalute_Content + "', "+ Rate + ", '"+ CurrentDate + "')";
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

    // 5. API Update a Evalute
    [WebMethod]
    public int UpdateEvalute(int EvaluteID, int CreateBy, string ProductID, string EvaluteContent, int Rate)
    {
        DateTime CurrentDate = DateTime.Now;

        string pattern = "'";
        string replacement = "''";
        string Evalute_content = Regex.Replace(EvaluteContent, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Evalute SET Create_by = " + CreateBy + ", Product_id = N'" + ProductID + "', Evalute_content = N'"+ Evalute_content  + "', Rate = "+ Rate + ", Create_at = '"+ CurrentDate + "' WHERE Evalute_id = " + EvaluteID;
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

    //------------------API For Event Table -----------------------

    //1 API get All Event Table
    [WebMethod]
    public DataSet GetAllEvent()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Event_id, Event_name, Discount FROM Event";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //2.Api get Event by Name
    [WebMethod]
    public DataSet GetEventByName(string EventName)
    {
        string pattern = "'";
        string replacement = "''";
        string Event_name = Regex.Replace(EventName, pattern, replacement);

        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Event_id, Event_name, Discount FROM Event WHERE Event_name LIKE N'%" + Event_name + "%'";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 3. API Insert a Event
    [WebMethod]
    public int InsertEvent(string EventName, float Discount)
    {
        
        string pattern = "'";
        string replacement = "''";
        string Event_name = Regex.Replace(EventName, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Event (Event_name, Discount) VALUES (N'" + Event_name + "', " + Discount +  ")";
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

    // 5. API Update a Event
    [WebMethod]
    public int UpdateEvent(int EventID, string EventName, float Discount)
    {
        string pattern = "'";
        string replacement = "''";
        string Event_name = Regex.Replace(EventName, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Event SET Event_name = N'" + Event_name + "', Discount = " + Discount +  " WHERE Event_id = " + EventID;
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

    // 6. API Delete a Evalute
    [WebMethod]
    public int DeleteEvent(int EventID)
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
            string sql = "DELETE FROM Event WHERE Event_id = " + EventID;
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


    //---------------- API for Order_detail Table ----------------------------

    //1. API get Order_detail by order_id ( get all column item of 3 table: order_detail, order, product)
    [WebMethod]
    public DataSet GetOrderDetailByOrderID(int  OrderID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Product.Product_id, Product.Product_name, Product.Description, Product.Image," +
            "  Product.Price, Product.Discount AS Discount_product, Product.Update_by, " +
            " [Order].Order_id, [Order].Status, [Order].Shipper_id,  " +
            " [Order].Flower_recipient_id, [Order].Create_at, [Order].Create_by," +
            "  [Order].Message_id, [Order].Receive_date, [Order].Receive_time, " +
            " Order_detail.Order_detail_id, Order_detail.Quantity,   Order_detail.Price AS Price_orderDetail " +
            " FROM  Product " +
            " RIGHT OUTER JOIN  Order_detail ON Product.Product_id = Order_detail.Product_id " +
            " LEFT OUTER JOIN  [Order] ON Order_detail.Order_id = [Order].Order_id WHERE [Order].Order_id = " + OrderID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API get distinct Receive_date
    [WebMethod]
    public DataSet GetReiveDate()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT DISTINCT Receive_time FROM [Order]";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);

        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //----------------- API For Order Table --------------------------------

    //1. API get all Order
    [WebMethod]
    public DataSet GetAllOrder()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Order_id, Status, Shipper_id, Flower_recipient_id, Create_at, Create_by, Message_id, Receive_date, Receive_time FROM [Order]";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }



    //---------------------API Supplier Table --------------------
    // 1. API Get All Supplier
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
    public DataSet GetAllSupplierByName(string SupplierName)
    {
        string pattern = "'";
        string replacement = "''";
        string Supplier_name = Regex.Replace(SupplierName, pattern, replacement);

        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Supplier_id,Supplier_name, Address, Phone FROM Supplier WHERE Supplier_name LIKE  N'%"+ Supplier_name + "%'", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    //3.API Insert Supplier
    [WebMethod]
    public int InsertSupplier(string SupplierName, string Address, string Phone )
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
            string sql = "INSERT INTO Supplier (Supplier_name, Address, Phone) VALUES (N'" + Supplier_name + "', N'"+ Address_ + "', '"+ Phone + "')";
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
            string sql = "UPDATE Supplier SET Supplier_name = N'" + Supplier_name + "', Address = N'"+ Address_ + "', Phone = '"+ Phone + "' WHERE Supplier_id = " + SupplierID;
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


    //----------------------API Banking Table ---------------------

    // 1. API Get All Banking
    [WebMethod]
    public DataSet GetAllBanking()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Banking_id, Banking_name, Number, Customer_id, Order_id, Total FROM Banking", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API Get All Banking By Supplier_name
    [WebMethod]
    public DataSet GetAllBankingByName(string BankingName)
    {
        string pattern = "'";
        string replacement = "''";
        string Banking_name = Regex.Replace(BankingName, pattern, replacement);

        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Banking_id, Banking_name, Number, Customer_id, Order_id, Total FROM Banking WHERE Banking_name LIKE  N'%" + Banking_name + "%'", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    //3.API Insert Banking
    [WebMethod]
    public int InsertBanking(string BankingName, string Number, int CustomerID, int OrderID, float Total)
    {
        string pattern = "'";
        string replacement = "''";
        string Banking_name = Regex.Replace(BankingName, pattern, replacement);
        string Number_ = Regex.Replace(Number, pattern, replacement);


        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO Banking (Banking_name, Number, Customer_id, Order_id, Total) VALUES (N'" + Banking_name + "', N'" + Number + "', " + CustomerID + ", "+ OrderID + ", "+ Total + ")";
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
    //4. API Update Banking
    [WebMethod]
    public int UpdateBanking(int BankingID, string BankingName, string Number, int CustomerID, int OrderID, float Total)
    {
        string pattern = "'";
        string replacement = "''";
        string Banking_name = Regex.Replace(BankingName, pattern, replacement);
        string Number_ = Regex.Replace(Number, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE Banking SET Banking_name = N'" + Banking_name + "', Number = N'" + Number_ + "', Customer_id = " + CustomerID + ", Order_id = "+ OrderID+ ", Total = "+ Total + " WHERE Banking_id = " + BankingID;
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

    //5.API Delete Banking
    [WebMethod]
    public int DeleteBanking(int BankingID)
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
            string sql = "DELETE FROM Banking WHERE Banking_id = " + BankingID;
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

    //------------------------API for Flower_recipient Table ----------------------

    // 1. API Get All Recipient
    [WebMethod]
    public DataSet GetAllRecipient()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Flower_recipient_id, Name, Address, Phone, Customer_id FROM Flower_recipient", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    // 2. API Get All Recipient By Search
    [WebMethod]
    public DataSet GetRecipientBySearch(string Search)
    {
        string pattern = "'";
        string replacement = "''";
        string Search_ = Regex.Replace(Search, pattern, replacement);

        SqlConnection cnn = new SqlConnection(connstr);
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Flower_recipient_id, Name, Address, Phone, Customer_id FROM Flower_recipient WHERE Name LIKE  N'%" + Search_ + "%' OR Address LIKE N'%" + Search_ + "%'", cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }


    //3.API Insert Recipient
    [WebMethod]
    public int InsertRecipient(string RecipientName, string Address,string Phone,int CustomerID)
    {
        string pattern = "'";
        string replacement = "''";
        string Name = Regex.Replace(RecipientName, pattern, replacement);
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
            string sql = "INSERT INTO Flower_recipient (Name, Address, Phone, Customer_id) VALUES (N'" + Name + "', N'" + Address_ + "', '" + Phone + "', " + CustomerID + ")";
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
    //4. API Update Flower_recipient
    [WebMethod]
    public int UpdateRecipient(int RecipientID, string RecipientName, string Address, string Phone, int CustomerID)
    {
        string pattern = "'";
        string replacement = "''";
        string Name = Regex.Replace(RecipientName, pattern, replacement);
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
            string sql = "UPDATE Flower_recipient SET Name = N'" + Name + "', Address = N'" + Address_ + "', Phone = '" + Phone + "', Customer_id = " + CustomerID + " WHERE Flower_recipient_id = " + RecipientID;
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

    //5.API Delete Flower_recipient
    [WebMethod]
    public int DeleteRecipient(int RecipientID)
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
            string sql = "DELETE FROM Flower_recipient WHERE Flower_recipient_id = " + RecipientID;
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



    //------------------------------API For Group Table ------------------------------

    //1.Api get All Group
    [WebMethod]
    public DataSet GetAllGroup()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Group_id, Group_name FROM [Group] ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //2. Api get Group by Group_id
    [WebMethod]
    public string GetGroupByID(int GroupID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Group_name FROM [Group]  Where  Group_id = " + GroupID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds.Tables[0].Rows[0]["Group_name"].ToString();
    }

    //3.API Insert Group
    [WebMethod]
    public int InsertGroup(string GroupName)
    {
        string pattern = "'";
        string replacement = "''";
        string Group_name = Regex.Replace(GroupName, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO [Group] (Group_name) VALUES (N'" + Group_name + "')";
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
    ////4. API Update Group
    [WebMethod]
    public int UpdateGroup(int GroupID, string GroupName)
    {
        string pattern = "'";
        string replacement = "''";
        string Group_name = Regex.Replace(GroupName, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE [Group] SET Group_name = N'" + Group_name + "' WHERE Group_id = " + GroupID;
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

    //5.API Delete Group
    [WebMethod]
    public int DeleteGroup(int GroupID)
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
            string sql = "DELETE FROM [Group] WHERE Group_id = " + GroupID;
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

    //------------------------------API For Function Table ------------------------------

    //1.Api get All Function
    [WebMethod]
    public DataSet GetAllFunction()
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Function_id, Function_name, Base_url FROM [Function] ";
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //2. Api get Function by Function_id
    [WebMethod]
    public DataSet GetFunctionByID(int FunctionID)
    {
        SqlConnection cnn = new SqlConnection(connstr);
        string sql = "SELECT Function_name, Base_url FROM [Function]  Where  Function_id = " + FunctionID;
        SqlDataAdapter adapter = new SqlDataAdapter(sql, cnn);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    //3.API Insert Function
    [WebMethod]
    public int InsertFunction(string FunctionName, string BaseUrl)
    {
        string pattern = "'";
        string replacement = "''";
        string Function_name = Regex.Replace(FunctionName, pattern, replacement);
        string Base_url = Regex.Replace(BaseUrl, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "INSERT INTO [Function] (Function_name, Base_url) VALUES (N'" + Function_name + "', N'"+ Base_url +"')";
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
    ////4. API Update Function
    [WebMethod]
    public int UpdateFunction(int FunctionID, string FunctionName, string BaseUrl)
    {
        string pattern = "'";
        string replacement = "''";
        string Function_name = Regex.Replace(FunctionName, pattern, replacement);
        string Base_url = Regex.Replace(BaseUrl, pattern, replacement);

        try
        {
            SqlConnection cnn = new SqlConnection(connstr);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            string sql = "UPDATE [Function] SET Function_name = N'" + Function_name + "', Base_url = N'"+ Base_url + "' WHERE Function_id = " + FunctionID;
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

    //5.API Delete Function
    [WebMethod]
    public int DeleteFunction(int FunctionID)
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
            string sql = "DELETE FROM [Function] WHERE Function_id = " + FunctionID;
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

}
