using Newtonsoft.Json;
using ProductStock.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductStock.Controllers
{
    public class HomeController : Controller
    {

        readonly public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString);
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string Insert(Entity e)
        {
            using (SqlCommand cmd = new SqlCommand("Product_list",con))
            {
                int id = 0;
                e.Product_ID = id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Product_ID", SqlDbType.Int).Value = id;
                cmd.Parameters.AddWithValue("@Product_Name", SqlDbType.VarChar).Value = e.Product_Name;
                cmd.Parameters.AddWithValue("@Price", SqlDbType.BigInt).Value = e.Price;
                cmd.Parameters.AddWithValue("@Total_stock", SqlDbType.BigInt).Value = e.Total_stock;
                cmd.Parameters.AddWithValue("@Total_price", SqlDbType.BigInt).Value = e.Total_Price;
                cmd.Parameters.AddWithValue("@Operation", SqlDbType.VarChar).Value = "Insert";

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return "success";
            }
        }

        public JsonResult Getalldata()
        {
            using (SqlCommand cmd = new SqlCommand("Select * from Product", con))
            {

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                var result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}