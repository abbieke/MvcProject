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
            int customerId;
            using (conn = new SqlConnection(connString))
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
        public Customer CustomerLogin(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= @account ;";
                var cust = conn.QueryFirstOrDefault<Customer>(sql,new { account});
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
        public void CustomerUpdateRole(UserRoles userRoles)
        {

            string sql = "UPDATE UserRoles SET RolesID= @RolesID WHERE UserID=@UserID ;";
            conn.Execute(sql, new { userRoles.RolesID, userRoles.UserID  });

        }

        public int GetCusromerID(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                int customerId= conn.Query<int>("GetCustomerID",
                                new { customerAccount =account },
                                commandType: CommandType.StoredProcedure
                                ).SingleOrDefault();
                return customerId;
            }
        }

        public void UpdateCustomer(CustomerViewModel cust)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Update Customers Set CustomerName=@CustomerName, CustomerEmail=@CustomerEmail, CustomerPhone=@CustomerPhone, CustomerAddress=@CustomerAddress WHERE CustomerAccount=@CustomerAccount";
                conn.Execute(sql, new {
                    CustomerName = cust.CustomerName,
                    CustomerAccount = cust.CustomerAccount,
                    CustomerEmail = cust.CustomerEmail,
                    CustomerPhone = cust.CustomerPhone,
                    CustomerAddress = cust.CustomerAddress});
            }

        }

        public void UpdateEmailConfirmed(int customerId,bool isConfirmed)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "UPDATE Customers SET EmailConfirmed= @isConfirmed WHERE CustomerID=@customerId;";
                conn.Execute(sql, new { isConfirmed , customerId });
            }
        }
        public void CustomerRemoveRole(int customerId, string roleId)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select RolesID from UserRoles where UserID=@customerId;";
                string result = conn.QueryFirstOrDefault<string>(sql,new { customerId });
                List<string> roles = result.Split(',').ToList();
                if (roles.IndexOf(roleId) != -1)
                {
                    roles.Remove(roleId);
                }
                
                UserRoles userRoles = new UserRoles
                {
                    UserID = customerId,
                    RolesID = string.Join(", ", roles.ToArray())
                };

                CustomerUpdateRole(userRoles);

            }
        }

        public void UpdatePassword(string account,string password)
        {
            using (conn = new SqlConnection(connString))
            {
                string sha256pwd = HashService.SHA256Hash(password);
                string sql = "UPDATE Customers SET CustomerPassword= @sha256pwd WHERE CustomerAccount=@account;";
                conn.Execute(sql, new { sha256pwd, account });
            }
        }

        public CustomerChangePasswordViewModel SelectCustomerPasswordView(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "'";
                var cust = conn.QueryFirstOrDefault<CustomerChangePasswordViewModel>(sql);
                return cust;

            }
        }

        public string SelectCustomerPassword(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerPassword From Customers Where CustomerAccount= '" + account + "'";
                var cust = conn.QueryFirstOrDefault<string>(sql);
                return cust;

            }
        }

        public List<Order> SelectOrders(string account)
        {
            List<Order> orders;
            using (conn = new SqlConnection(connString))
            {
                string sql =
                    "Select * from Orders As o Inner Join Customers As c On o.CustomerID = c.CustomerID Inner Join[Order Status] As os On o.OrderID = os.OrderID Where c.CustomerAccount = '" +
                    account + "'";
                orders = conn.Query<Order>(sql).ToList();
                return orders;
            }
        }

        public Order SelectOrder(int? orderId)
        {
            Order orders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * from Orders Where OrderID = " + orderId;
                orders = conn.QueryFirstOrDefault<Order>(sql);
                return orders;
            }
        }

        public List<OrderDetailModel> SelectOrderDetails(int? orderId)
        {
            List<OrderDetailModel> orderDetails;
            using (conn = new SqlConnection(connString))
            {
                string sql =
                    "Select * from [Order Detail] as od Inner join Books As b On od.BooksNo = b.BooksNo Where od.OrderID = " + orderId;
                orderDetails = conn.Query<OrderDetailModel>(sql).ToList();
                return orderDetails;
            }
        }

        public OrderStatusModel SelectOrderStatus(int? orderId)
        {
            OrderStatusModel orders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * from [Order Status] Where OrderID = " + orderId;
                orders = conn.QueryFirstOrDefault<OrderStatusModel>(sql);
                return orders;
            }
        }
        public List<string> SelectRoles(int userid)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select RolesID from UserRoles where UserID=@userid";
                string result = conn.QueryFirstOrDefault<string>(sql,new { userid });
                string[] rolesArr= result.Split(',');
                return rolesArr.ToList();
            }
        }
        public string SelectRolesName(string roleid)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select RoleName from Roles where RoleId=@roleid";
                string roleName = conn.QueryFirstOrDefault<string>(sql, new { roleid });
                return roleName;
            }
        }
    }
}