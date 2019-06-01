﻿using Dapper;
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

        public List<ShoppingCartViewModel> SelectCart(int customerID)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select sc.BookID,Quantity,BooksNo,b.BooksName,b.UnitPrice,b.InStock,b.Discount,b.BookImage,c.CategoryEngName from [Shopping Car] sc inner join Books b on sc.BookID=b.BookID inner join Category c on b.CategoryID=c.CategoryID where CustomerID=@customerID;";
                var carts = conn.Query<ShoppingCartViewModel>(sql,new { customerID }).ToList();
                return carts;
            }
        }

        //test
        public void RemoveCartBook(int customerId, string bookId)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "DELETE FROM [Shopping Car] WHERE CustomerID = @cusId AND BookID = @bookid";
                var carts = conn.Query<ShoppingCartViewModel>(sql, new { cusId=customerId, bookid = bookId  }).ToList();
                
            }
        }
    }
}