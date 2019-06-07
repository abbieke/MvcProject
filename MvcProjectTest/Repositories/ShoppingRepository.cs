using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using MvcProjectTest.Models;
using MvcProjectTest.ViewModels;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Repositories
{
    public class ShoppingRepository
    {
        private static string connString;
        private SqlConnection conn;
        private readonly CustomersRepository _cusRepo;
        public ShoppingRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["bsmobile"].ConnectionString;
            }

            conn = new SqlConnection(connString);
            _cusRepo = new CustomersRepository();
        }

        public IEnumerable<ShoppingCar> GetMemberCart(string account)
        {
            
            using (conn = new SqlConnection(connString))
            {
                var sql = "SELECT * FROM[Shopping Car] pc INNER JOIN Customers c ON  pc.CustomerID = c.CustomerID WHERE c.CustomerAccount = @account";
                return conn.Query<ShoppingCar>(sql, new { account });
            }
        }

        public List<ShoppingCartViewModel> SelectCart(int customerID)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select sc.BookID,Quantity,BooksNo,b.BooksName,b.UnitPrice,b.InStock,b.Discount,b.BookImage,c.CategoryEngName from [Shopping Car] sc inner join Books b on sc.BookID=b.BookID inner join Category c on b.CategoryID=c.CategoryID where CustomerID=@customerID;";
                var carts = conn.Query<ShoppingCartViewModel>(sql,new { customerID }).ToList();
                return carts;
            }
        }
        public ShoppingCartViewModel SelectCart(int customerID, string bookId)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select sc.BookID,Quantity,BooksNo,b.BooksName,b.UnitPrice,b.InStock,b.Discount,b.BookImage,c.CategoryEngName from [Shopping Car] sc inner join Books b on sc.BookID=b.BookID inner join Category c on b.CategoryID=c.CategoryID where CustomerID=@customerID and  b.BookID=@bookId;";
                var carts = conn.QueryFirstOrDefault<ShoppingCartViewModel>(sql, new { customerID, bookId });
                return carts;
            }
        }

        public void InsertCartBook(int customerId, string bookId,int qty)
        {
            var cart = SelectCart(customerId, bookId);

            if (cart == null)
            {
                using (conn = new SqlConnection(connString))
                {
                    string sql = "INSERT INTO [Shopping Car](CustomerID, BookID, Quantity) VALUES (@cusId,@bookid,@quantity) ";
                    conn.Execute(sql, new { cusId = customerId, bookid = bookId, quantity = qty });
                }
            }
            else
            {
                qty = qty + cart.Quantity;
                using (conn = new SqlConnection(connString))
                {                 
                    string sql = "UPDATE[Shopping Car] SET Quantity = @quantity WHERE CustomerID = @cusId and BookID = @bookid;" ;
                    conn.Execute(sql, new { cusId = customerId, bookid = bookId, quantity = qty });
                }
            }
            
        }

        public void DeleteByAccount(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                var id = _cusRepo.GetCusromerID(account);
                var sql = "DELETE FROM [Shopping Car] WHERE CustomerID = @customerID";
                conn.Execute(sql, new { customerID = id });
            }
            
        }

        //test
        public void RemoveCartBook(int customerId, string bookId)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "DELETE FROM [Shopping Car] WHERE CustomerID = @cusId AND BookID = @bookid";
                //var carts = conn.Query<ShoppingCartViewModel>(sql, new { cusId=customerId, bookid = bookId  }).ToList();
                var carts = conn.Execute(sql, new { cusId = customerId, bookid = bookId });
            }
        }
        public bool SelectWish(int customerId, string bookId)
        {
            
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select Count(*) From WishList WHERE CustomerID = @cusId AND BookID = @bookid ";
                int count=conn.Query<int>(sql, new { cusId = customerId, bookid = bookId }).FirstOrDefault();
                if (count == 0) { return false; }
                else { return true; }
            }
        }

        public List<Book> SelectWish(int customerId)
        {

            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From WishList w inner join Books b on w.BookID=b.BookID inner join Category c on b.CategoryID=c.CategoryID WHERE CustomerID = @cusId Order By WishDate DESC;";
                List<Book> books = conn.Query<Book>(sql, new { cusId = customerId}).ToList();
                return books;
            }
        }


        public void InsertWish(int customerId, string bookId)
        {          
            using (conn = new SqlConnection(connString))
            {
                string sql = "INSERT INTO WishList(CustomerID, BookID, WishDate) VALUES (@cusId,@bookid,@wishdate) ";
                conn.Execute(sql, new { cusId = customerId, bookid = bookId, wishdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            }
        }

        public void RemoveWish(int customerId, string bookId)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "DELETE FROM WishList WHERE CustomerID = @cusId AND BookID = @bookid";
                conn.Execute(sql, new { cusId = customerId, bookid = bookId });
            }
        }
    }
}