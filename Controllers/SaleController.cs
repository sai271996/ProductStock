using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using ProductStock.Models;
using Microsoft.Ajax.Utilities;
using System.Security.Policy;
using System.ComponentModel;
using System.Web.DynamicData;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Drawing;

namespace ProductStock.Controllers
{
    public class SaleController : Controller
    {
        readonly public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBcon"].ConnectionString);
        
        public ActionResult buy()
        {
            product_bind();
            return View();
        }
        public ActionResult BuyList()
        {
            return View();
        }


        public JsonResult Getsaledata()
        {
            using (SqlCommand cmd = new SqlCommand("Select Product_Id, Product_Name,Price, Stocks_buy,Total_price,CONVERT(VARCHAR, Buy_date, 103) as Buy_date from sale", con))
            {

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                
                var result =  JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public DataSet product()
        {
            using (SqlCommand cmd = new SqlCommand("select * from product", con))
            {
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                return ds;
            }

        }
        public void product_bind()
        {
            DataSet ds  = product();
            List<SelectListItem> productlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                productlist.Add(new SelectListItem { Text = dr["Product_Name"].ToString(), Value = dr["Product_ID"].ToString() });

            }
            ViewBag.product = productlist;
      
        }
        public string Update(Entity en)
        {
            using (SqlCommand cmd = new SqlCommand("Product_list", con))
            {
              
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Product_Id", SqlDbType.Int).Value = en.Product_ID;
                cmd.Parameters.AddWithValue("@Product_Name", SqlDbType.VarChar).Value = en.Product_Name;
                cmd.Parameters.AddWithValue("@Price", SqlDbType.BigInt).Value = en.Price;
                cmd.Parameters.AddWithValue("@Total_stock", SqlDbType.BigInt).Value = en.Total_stock;
                cmd.Parameters.AddWithValue("@Total_price", SqlDbType.BigInt).Value = en.Total_Price;
                cmd.Parameters.AddWithValue("@Operation", SqlDbType.VarChar).Value = "Update";

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                return "success";
            }
        }
        public JsonResult proudctprice_Totalstock_bind(Entity e)
        {
            int id = e.Product_ID;
            using (SqlCommand cmd = new SqlCommand("select  Price, Total_stock from Product where Product_ID = "+ id , con))
            {
                SqlDataAdapter sda = new SqlDataAdapter(cmd); 
                DataTable dt = new DataTable();
                sda.Fill(dt);
                var result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        public string buyproduct(Sale en)
        {
            using (SqlCommand cmd = new SqlCommand("BUY", con))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Product_Id", SqlDbType.Int).Value = en.Product_Id;
                cmd.Parameters.AddWithValue("@Product_Name", SqlDbType.VarChar).Value = en.Product_Name;
                cmd.Parameters.AddWithValue("@Price", SqlDbType.BigInt).Value = en.Price;
                cmd.Parameters.AddWithValue("@Stocks_buy", SqlDbType.BigInt).Value = en.Stocks_buy;
                cmd.Parameters.AddWithValue("@Total_price", SqlDbType.BigInt).Value = en.Total_price;
                cmd.Parameters.AddWithValue("@Buy_date", SqlDbType.Date).Value = en.Buy_date;
                cmd.Parameters.AddWithValue("@Operation", SqlDbType.VarChar).Value = "sale";
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                return "success";
            }

        }
        public FileResult DownloadExcel(Sale s)
        {
           //DataTable dt = (DataTable)Session["sale"];

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {

                SqlCommand cmd = new SqlCommand("SELECT Product_Id, Product_Name,Price,Stocks_buy,Total_price,CONVERT(VARCHAR, Buy_date, 105) as Buy_date FROM sale WHERE DAY(Buy_date) = DAY(GETDATE()) AND MONTH(Buy_date) = MONTH(GETDATE())", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt1 = new DataTable();
                sda.Fill(dt1);

                SqlCommand cmd1 = new SqlCommand("SELECT Product_Id, Product_Name,Price,Stocks_buy,Total_price,CONVERT(VARCHAR, Buy_date, 105) as Buy_date FROM sale WHERE MONTH(Buy_date) = MONTH(GETDATE())", con);
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                DataTable dt2 = new DataTable();
                sda1.Fill(dt2);

                SqlCommand cmd2 = new SqlCommand("BUY", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Product_Id", SqlDbType.Int).Value = 0;
                cmd2.Parameters.AddWithValue("@Product_Name", SqlDbType.VarChar).Value = "";
                cmd2.Parameters.AddWithValue("@Price", SqlDbType.BigInt).Value = 0;
                cmd2.Parameters.AddWithValue("@Stocks_buy", SqlDbType.BigInt).Value = 0;
                cmd2.Parameters.AddWithValue("@Total_price", SqlDbType.BigInt).Value = 0;
                cmd2.Parameters.AddWithValue("@Buy_date", SqlDbType.VarChar).Value = "";
                cmd2.Parameters.AddWithValue("@operation", SqlDbType.VarChar).Value = "Week";
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                DataTable dt3 = new DataTable();
                sda2.Fill(dt3);


                var worksheet = package.Workbook.Worksheets.Add("Daily_Report");
                var worksheet1 = package.Workbook.Worksheets.Add("Monthly_Report");
                //var worksheet2 = package.Workbook.Worksheets.Add("Weekly_Report");


                // Fill the worksheet with data from the DataTable
                worksheet.Cells["A1"].LoadFromDataTable(dt1, true);
                worksheet1.Cells["A1"].LoadFromDataTable(dt2, true);
                worksheet2.Cells["A1"].LoadFromDataTable(dt3, true);

                worksheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);

                worksheet1.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet1.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);

                worksheet2.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet2.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange)

                var headerRow = worksheet.Cells["A1:F1"];

                // Apply the AutoFilter to the header row.                 
                headerRow.AutoFilter = true;

                // Save the changes to the Excel file.
                package.Save();

                // Return the Excel file as a FileResult
                byte[] fileBytes = package.GetAsByteArray();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sale_Report.xlsx");
            }
        }
        
    }
}
