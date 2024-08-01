using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management_System.Models
{
    public class Item
    {
        public int item_id { get; set; }

        public string item_name { get; set; }

        public string Category { get; set;}

        public int rate { get; set; }

        public int balance_Quantity { get; set; }


    }
}