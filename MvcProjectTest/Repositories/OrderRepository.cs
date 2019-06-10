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


        //加訂單寫在這之下

        public Order CreateOrder(Order model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "放你的SQL語法小老鼠就是變數名稱 大概像下面那樣可以運用Model傳進變數 @aa";
                var order = conn.QueryFirstOrDefault<Order>(sql);
                return order;

            }
        }

        public void CreateOrderStatus(Order model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "只要傳orderID跟setup哦 其他不用裡他應該可以null 不行去群組喊救命 @aa";
                conn.Execute(sql, new { aa = model.SetUp });

            }
        }

        public void CreateOrderDetail(IEnumerable<ViewModels.ShoppingCartViewModel> model)
        {
            using (conn = new SqlConnection(connString))
            {
                foreach(var item in model)
                {
                    string sql = "他的count看起來像是..Quantity? 先把Quantity傳過去 @aa";
                    conn.Execute(sql, new { aa = item.Quantity });
                }
                

            }
        }


    }
}