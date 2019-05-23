using Dapper;
using MvcProjectTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Repositories
{
    public class CustomersRepository
    {
        private static string connString;
        private SqlConnection conn;
        public CustomersRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["bsmobile"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        public void InsertCustomer(Customer cust)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "INSERT INTO Customers(CustomerName, CustomerAccount, CustomerPassword, CustomerEmail, CustomerPhone, CustomerAddress, CustomerBirth) VALUES ( @CustomerName, @CustomerAccount, @CustomerPassword, @CustomerEmail, @CustomerPhone, @CustomerAddress, @CustomerBirth)";
                conn.Execute(sql, new { cust.CustomerName,cust.CustomerAccount, cust.CustomerPassword, cust.CustomerEmail, cust.CustomerPhone, cust.CustomerAddress, cust.CustomerBirth });
            }
        }

        public bool SelectCustomer(string CustomerAccount)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerAccount From Customers Where CustomerAccount = '" + CustomerAccount + "'";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                if(cust == null)
                {
                    return true;
                }
                else { return false; }
            }
        }

        public bool SelectCustomerEmail(string CustomerEmail)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerEmail From Customers Where CustomerEmail = '" + CustomerEmail + "'";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                if (cust == null)
                {
                    return true;
                }
                else { return false; }
            }
        }
        public Customer CustomerLogin(string account, string password)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "' and CustomerPassword= '" + password + "';";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                return cust;

            }
        }

        public CustomerViewModel SelectCustomerView(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "'";
                var cust = conn.QueryFirstOrDefault<CustomerViewModel>(sql);
                return cust;

            }
        }

        public void UpdateCustomer(CustomerViewModel cust)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Update Customers Set CustomerName=@CustomerName, CustomerEmail=@CustomerEmail, CustomerPhone=@CustomerPhone, CustomerAddress=@CustomerAddress, CustomerBirth=@CustomerBirth WHERE CustomerAccount = @CustomerAccount";
                conn.Execute(sql, new {
                    CustomerName = cust.CustomerName,
                    CustomerAccount = cust.CustomerAccount,
                    CustomerEmail = cust.CustomerEmail,
                    CustomerPhone = cust.CustomerPhone,
                    CustomerAddress = cust.CustomerAddress,
                    CustomerBirth = cust.CustomerBirth });
            }
        }
    }
}