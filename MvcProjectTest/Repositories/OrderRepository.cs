using Dapper;
using MvcProjectTest.Models;
using MvcProjectTest.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Repositories
{
    public class OrderRepository
    {
        private static string connString;
        private SqlConnection conn;
        public OrderRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["bsmobile"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        public string GetMaxNo()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "SELECT MAX(OrderNO) FROM Orders";
                var maxNo = conn.QueryFirstOrDefault<string>(sql);
                return maxNo;

            }
        }
    }
}