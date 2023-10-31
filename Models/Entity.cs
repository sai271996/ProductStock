using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductStock.Models
{
    public class Entity
    {
        public int Product_ID { get; set; } 
        public string Product_Name { get; set; }
        public int Price { get; set; }
        public int Total_stock { get; set; }
        public int Total_Price { get; set; }
    }
}