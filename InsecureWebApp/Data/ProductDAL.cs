using Microsoft.Extensions.Configuration;
using MicroFocus.InsecureWebApp.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System;

namespace MicroFocus.InsecureWebApp.Data
{
    public class ProductDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public ProductDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connectionString = this._configuration.GetConnectionString("DefaultConnection");
        }

        public List<Product> GetAllProduct(string sSearchText)
        {
            string sqlText = string.Empty;
            sqlText = "SELECT Top 100 * FROM Product where name like '%" + sSearchText + "%'";
            var lstProducts = new List<Product>();
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlText, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstProducts.Add(new Product
                        {
                            ID = reader.GetInt32("ID"),
                            Code = reader.GetString("Code"),
                            Name = reader.GetString("Name"),
                            Summary = reader.GetString("Summary"),
                            Description = reader.GetString("Description"),
                            Image = reader.GetString("Image"),
                            Price = reader.GetDecimal("Price")
                        });
                    }
                }
            }
            return lstProducts;

            //try
            //{
            //    using (SqlConnection con = new SqlConnection(_connectionString))
            //    {
            //        SqlCommand cmd = new SqlCommand(sqlText, con);
            //        cmd.CommandType = CommandType.Text;
            //        con.Open();
            //        SqlDataReader rdr = cmd.ExecuteReader();
            //        while (rdr.Read())
            //        {
            //            lstProducts.Add(new Product
            //            {
            //                ID = rdr.GetInt32("ID"),
            //                Code = rdr.GetString("Code"),
            //                Name = rdr.GetString("Name"),
            //                Summary = rdr.GetString("Summary"),
            //                Description = rdr.GetString("Description"),
            //                Image = rdr.GetString("Image"),
            //                Price = rdr.GetDecimal("Price")
            //            }) ;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return lstProducts;
        }
    }
}
