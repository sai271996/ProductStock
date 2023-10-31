using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductStock.Models
{
    public class Sale
    {
        public int Product_Id { get; set; }
        public string  Product_Name { get; set; }
        public int Price { get; set; }
        public int Stocks_buy { get; set; }
        public int Total_price { get; set; }
        public DateTime Buy_date { get; set; }

    }
}