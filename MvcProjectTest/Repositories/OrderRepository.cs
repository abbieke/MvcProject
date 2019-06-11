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

        public void CreateOrder(Order model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into Orders (OrderNo,CustomerID,OrderDate,PayWay,DeliveryMethod,TotalPrice,Recipient,RecipientPhone,RecipientEmail,RecipientAddress,ShippingRate) values(@OrderNo,@CustomerID,@OrderData,@PayWay,@DeliveryMethod,@TotalPrice,@Recipient,@RecipientPhone,@RecipientEmail,@RecipientAddress,@ShippingRate)";
                conn.Execute(sql, new {

                    OrderNo = model.OrderNo,
                    CustomerID = model.CustomerID,
                    OrderData = model.OrderDate,
                    PayWay = model.PayWay,
                    DeliveryMethod = model.DeliveryMethod,

                    TotalPrice = model.TotalPrice,
                    Recipient = model.Recipient,
                    RecipientPhone = model.RecipientPhone,
                    RecipientEmail = model.RecipientEmail,
                    RecipientAddress = model.RecipientAddress,
                    
                    ShippingRate = model.ShippingRate
                });
            }
        }

        public void CreateOrderStatus(int orderID,Order model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into[dbo].[Order Status](OrderID,SetUp) values(@OrderID,@SetUp)";
                conn.Execute(sql, new {

                    OrderID =orderID,
                    SetUp =model.SetUp

                });

            }
        }

        public void CreateOrderDetail(int orderID, IEnumerable<ViewModels.ShoppingCartViewModel> model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into[dbo].[Order Detail](OrderID,BookID,UnitPrice,Counts,Discount) values(@OrderID,@BookID,@UnitPrice,@Counts,@Discount)";
                foreach (var item in model)
                {
                    conn.Execute(sql, new
                    {
                        OrderID=orderID,
                        BookID=item.BookID,
                        UnitPrice=item.UnitPrice,
                        Counts=item.Quantity,
                        Discount=item.Discount
                    });
                }

            }
        }

        public Order GetOrderFromOrderNo(string orderNo)
        {
            Order result;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from[dbo].[Orders] where OrderNo ='" + orderNo + "'";
                result = conn.QueryFirstOrDefault<Order>(sql);
            }
            return result;
        }

        public IEnumerable<Order> GetOrderDetail()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from[dbo].[Order Detail]";
                var result = conn.Query<Order>(sql);
                return result;
            }
            
        }
    }
}