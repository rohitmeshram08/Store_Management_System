﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management_System.Models
{
    public class Transaction
    {
        public int transaction_ID { get; set; }

        public string transaction_name { get; set;}

        public DateTime transaction_date { get; set;}

        public string Item_Name { get; set;}

        public string Item_ID { get; set;}



        public int department_id { get; set; }
        public string Department_Name { get; set; }

        public int Vendor_id { get; set; }
        public string Vendor_Name { get; set; }



        public int Quantity { get; set; }




    }
}