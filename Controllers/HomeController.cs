﻿using Store_Management_System.Models;
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

        public ActionResult Transction()
        {

            
            ViewBag.TransactionDetails = Get_transactionDetails();

            ViewBag.Item_List = Get_item_Data();

            return View();
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
    }
}