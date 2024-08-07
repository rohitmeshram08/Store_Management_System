using Store_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public string Connection_string= System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

        public SqlConnection connection;
        public SqlCommand command;

        public ActionResult Index(Item model)
        {
            if (model.item_id == 0)
            {
                 List<SelectListItem> list = new List<SelectListItem>()
                 {
                    new SelectListItem(){Text="ELECTRONICS",Value="ELECTRONICS"},
                    new SelectListItem(){Text="STATIONARY",Value="STATIONARY"},
                    new SelectListItem(){Text="CONSUMABLE",Value="CONSUMABLE"},
                    new SelectListItem(){Text="FURNITURE",Value="FURNITURE"},
                };
                ViewBag.Category_list = list;

                ViewBag.Item_List = Get_item_Data();
                return View();
            }
            else 
            {

                List<SelectListItem> list = new List<SelectListItem>()
                 {
                    new SelectListItem(){Text="ELECTRONICS",Value="ELECTRONICS"},
                    new SelectListItem(){Text="STATIONARY",Value="STATIONARY"},
                    new SelectListItem(){Text="CONSUMABLE",Value="CONSUMABLE"},
                    new SelectListItem(){Text="FURNITURE",Value="FURNITURE"},
                };
                ViewBag.Category_list = list;
                ViewBag.Item_List = Get_item_Data();

                string Query = $"SELECT *FROM tbl_item where item_id={model.item_id}";

                connection= new SqlConnection(Connection_string);

                command = new SqlCommand(Query, connection);

                SqlDataAdapter DA= new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                Item item_model= new Item();

                DA.Fill(dt);

                foreach(DataRow dataRow in dt.Rows)
                {
                    item_model.item_id = Convert.ToInt32(dataRow["item_id"]);
                    item_model.rate= Convert.ToInt32(dataRow["rate"]);
                    item_model.balance_Quantity = Convert.ToInt32(dataRow["balance_quantity"]);
                    item_model.Category = Convert.ToString(dataRow["Category"]);
                    item_model.item_name = Convert.ToString(dataRow["item_name"]);
                }
                return View(item_model);
            }
        }


        [HttpPost]
        public ActionResult AddorUpdate(Item model)
        {
            if(model.item_id == 0)
            {
                string Query = $"INSERT INTO tbl_item VALUES ('{model.item_name}','{model.Category}',{model.rate},{model.balance_Quantity})";

                connection = new SqlConnection(Connection_string);

                command = new SqlCommand(Query, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public List<Item> Get_item_Data()
        {
            connection = new SqlConnection(Connection_string);

            string Query = "SELECT * FROM tbl_item ";

            command = new SqlCommand(Query, connection);

            List<Item> Item_list = new List<Item>();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);

            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                Item item = new Item()
                {
                    item_id = Convert.ToInt32(dr["item_id"]),
                    item_name = Convert.ToString(dr["item_name"]),
                    Category= Convert.ToString(dr["Category"]),
                    rate = Convert.ToInt32(dr["rate"]),
                    balance_Quantity = Convert.ToInt32(dr["balance_quantity"]),
                };
                Item_list.Add(item);  
            }
            return Item_list;
        }

        public ActionResult Delete(int item_id)
        {
            connection = new SqlConnection(Connection_string);

            string query = $"DELETE FROM tbl_item WHERE item_id={item_id} ";

            command = new SqlCommand(query, connection);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToAction("Index");
        }

        public ActionResult Transction( Transaction transaction_Model)
        {
            if(transaction_Model.transaction_ID== 0)
            {
                ViewBag.TransactionDetails = Get_transactionDetails();

                ViewBag.Department_List = GetDepartment_Details();

                return View();

            }
            else
            {
                ViewBag.TransactionDetails = Get_transactionDetails();

                ViewBag.Department_List = GetDepartment_Details();

                connection = new SqlConnection (Connection_string);

                string Query = $"SELECT transaction_id as 'TRANSACTION ID',I.item_name as 'ITEM NAME' ,T.transaction_date as 'TRANSACTION DATE',D.department_id as 'DepartmentId',D.Department_name as 'DepartmentName',V.Vendor_name as 'VENDOR NAME',T.Quantity as 'QUANTITY'FROM tbl_transaction as T inner join tbl_item as I on T.item_id=I.item_id inner join tbl_department as D on T.department_id=D.department_id inner join tbl_vendor_master as V on  V.Vendor_id=T.Vendor_id  where transaction_id={transaction_Model.transaction_ID} ";

                command = new SqlCommand(Query, connection);

                SqlDataAdapter AD = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                Transaction transaction = new Transaction();

                AD.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    transaction.transaction_ID = Convert.ToInt32(dr["TRANSACTION ID"]);
                    transaction.department_id = Convert.ToInt32(dr["DepartmentId"]);
                    transaction.Vendor_Name = Convert.ToString(dr["VENDOR NAME"]);
                    transaction.Quantity = Convert.ToInt32(dr["QUANTITY"]);
                    transaction.Department_Name = Convert.ToString(dr["DepartmentName"]);
                }
                return View(transaction);

            }  
        }

        public List<Transaction> Get_transactionDetails()
        {

            connection= new SqlConnection(Connection_string);

            string Query = "SELECT transaction_id as 'TRANSACTION ID',I.item_name as 'ITEM NAME' ,T.transaction_date as 'TRANSACTION DATE',\r\nD.Department_name as 'DEPARTMENT NAME',V.Vendor_name as 'VENDOR NAME',T.Quantity as 'QUANTITY'\r\nFROM tbl_transaction as T inner join \r\ntbl_item as I on T.item_id=I.item_id inner join \r\ntbl_department as D on T.department_id=D.department_id inner join \r\ntbl_vendor_master as V on  V.Vendor_id=T.Vendor_id ";

            command = new SqlCommand(Query, connection);

            List<Transaction> transactionsList = new List<Transaction>();


            DataTable dataTable = new DataTable();  

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                Transaction transaction = new Transaction()
                {
                    transaction_ID = Convert.ToInt32(row["TRANSACTION ID"]),
                    Item_Name = Convert.ToString(row["ITEM NAME"]),
                    transaction_date = Convert.ToDateTime(row["TRANSACTION DATE"]),
                    Department_Name = Convert.ToString(row["DEPARTMENT NAME"]),
                    Vendor_Name= Convert.ToString(row["VENDOR NAME"]),
                    Quantity = Convert.ToInt32(row["QUANTITY"]),
                };
                transactionsList.Add(transaction);
            }
            return transactionsList;
        }


        public List<Department> GetDepartment_Details()
        {

            connection = new SqlConnection(Connection_string);

            string Query = "SELECT * FROM tbl_department";
 

            command = new SqlCommand(Query, connection);

            List<Department> Department_details_LIST = new List<Department>();


            DataTable dataTable = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                Department department = new Department() 
                {
                    DepartmentId = Convert.ToInt32(row["department_id"]),
                    DepartmentName = Convert.ToString(row["Department_name"]),
                    
                };
                Department_details_LIST.Add(department);
            }
            return Department_details_LIST;
        }

        public List<Vendor> Get_Vendor_Details()
        {

            connection = new SqlConnection(Connection_string);

            string Query = "SELECT * FROM tbl_department";

            command = new SqlCommand(Query, connection);

            List<Vendor> Vendor_details_LIST = new List<Vendor>();


            DataTable dataTable = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                Vendor vendor= new Vendor()
                {
                    Vendor_Id = Convert.ToInt32(row["Vendor_id"]),
                    Vendor_Name = Convert.ToString(row["Vendor_name"]),
                };
                Vendor_details_LIST.Add(vendor);
            }
            return Vendor_details_LIST;
        }


        public ActionResult AddOrUpdate(Transaction transaction)
        {
            if(transaction.transaction_ID == 0)
            {
                connection= new SqlConnection(Connection_string);

                string query = $"INSERT INTO tbl_transaction VALUES({transaction.Item_ID},'{transaction.transaction_date}',{transaction.department_id},{transaction.Vendor_id},{transaction.Quantity})";

                command = new SqlCommand(query, connection);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();

                return RedirectToAction("Transction");


            }
            else
            {
                connection = new SqlConnection (Connection_string);

                string Query = $"";

                command = new SqlCommand(Query, connection);

                connection.Open ();

                command.ExecuteNonQuery();

                connection.Close();

                return RedirectToAction("Transction");

            }
        }
    }
}