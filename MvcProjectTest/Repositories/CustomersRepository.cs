using Dapper;
using MvcProjectTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcProjectTest.Repositories
{
    public class CustomersRepository
    {
        private static string connString;
        private readonly SqlConnection conn;
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
            int customerId;
            using (conn)
            {
                string sql = "INSERT INTO Customers(CustomerName, CustomerAccount, CustomerPassword, CustomerEmail, CustomerPhone, CustomerAddress, CustomerBirth) VALUES ( @CustomerName, @CustomerAccount, @CustomerPassword, @CustomerEmail, @CustomerPhone, @CustomerAddress, @CustomerBirth)";
                conn.Execute(sql, new { cust.CustomerName,cust.CustomerAccount, cust.CustomerPassword, cust.CustomerEmail, cust.CustomerPhone, cust.CustomerAddress, cust.CustomerBirth });
                //ROLE TEST
                customerId = conn.Query<int>("GetCustomerID",
                                new { customerAccount = cust.CustomerAccount },
                                commandType: CommandType.StoredProcedure
                                ).SingleOrDefault();
                UserRoles userRoles = new UserRoles
                {
                    UserID = customerId,
                    RolesID = "1,4"
                };
                CustomerAddRole(userRoles);
            }
            
        }

        public bool SelectCustomer(string CustomerAccount)
        {
            using (conn)
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
            using (conn)
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
            using (conn)
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "' and CustomerPassword= '" + password + "';";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                return cust;

            }
        }

        public bool IsEmailConfirmed(string account)
        {
            using (conn)
            {
                string sql = "Select EmailConfirmed From Customers Where CustomerAccount= '" + account + "' ;";
                var result = conn.QueryFirstOrDefault<bool>(sql);
                return result;

            }
        }
        public void CustomerAddRole(UserRoles userRoles)
        {
            
                string sql = "INSERT INTO UserRoles(UserID,RolesID) VALUES ( @UserID,@RolesID) ;";
                conn.Execute(sql, new {userRoles.UserID, userRoles.RolesID });
            
        }

        public int GetCusromerID(string account)
        {
            using (conn)
            {
                int customerId= conn.Query<int>("GetCustomerID",
                                new { customerAccount =account },
                                commandType: CommandType.StoredProcedure
                                ).SingleOrDefault();
                return customerId;
            }
        }

    }
}