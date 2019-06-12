using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcProjectTest.Repositories;
using MvcProjectTest.Services;
using MvcProjectTest.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MvcProjectTest.Models;

namespace MvcProjectTest.Controllers
{
    public class BackStageController : Controller
    {
        private readonly CustomersRepository _cusRepo;
        private readonly OrderRepository _orderRepo;
        public BackStageController()
        {
            _cusRepo = new CustomersRepository();
            _orderRepo = new OrderRepository();
        }
        // GET: BackStage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerDetails(string id)
        {
            var CustDetail = _cusRepo.SelectCustomerDetail(id);
            var CustCount = _cusRepo.SelectCustomerOrderCount(id);
            CustomerDetail mix = new CustomerDetail(){ Customer = CustDetail, OrderCount = CustCount};
            return View(mix);
        }

        public ActionResult CustomerEdit(string id)
        {
            var CustDetail = _cusRepo.SelectCustomerView(id);
            int cust_id = _cusRepo.GetCusromerID(id);
            List<string> cust_roles = _cusRepo.SelectRoles(cust_id);

            ViewBag.isAdmin = cust_roles.Contains("2");
            return View(CustDetail);
        }

        public ActionResult CustomerIndex()
        {
            var Customers = _cusRepo.ReadAllCustomer();
            List<Customer> cust = new List<Customer>();
            cust = Customers.ToList();
            return View(cust);
        }

        public ActionResult CustomerUpdate(CustomerViewModel cust)
        {
            var current_cust = _cusRepo.SelectCustomerDetail(cust.CustomerAccount);
            if (current_cust.EmailConfirmed != cust.EmailConfirmed)
            {
                if (cust.EmailConfirmed)
                {
                    _cusRepo.CustomerRemoveRole(cust.CustomerId,"4");
                }
                else
                {
                    List<string> current_roles = _cusRepo.SelectRoles(cust.CustomerId);
                    current_roles.Add("4");
                    string str_current_roles = String.Join(",", current_roles.ToArray());
                    UserRoles userRoles = new UserRoles
                    {

                        UserID = cust.CustomerId,
                        RolesID = str_current_roles
                    };
                    _cusRepo.CustomerUpdateRole(userRoles);
                }

            }
            _cusRepo.UpdateCustomer(cust);
            
            return RedirectToAction("CustomerIndex", "BackStage");
        }
        public ActionResult UpdateAdminRole(string custid, bool isAdmin)
        {
            int int_custid = Convert.ToInt32(custid);
            List<string> current_roles = _cusRepo.SelectRoles(int_custid);
            if (current_roles.Contains("2") != isAdmin)
            {
                if (isAdmin)
                {
                    current_roles.Add("2");
                    string str_current_roles = String.Join(",", current_roles.ToArray());
                    UserRoles userRoles = new UserRoles
                    {

                        UserID = int_custid,
                        RolesID = str_current_roles
                    };
                    _cusRepo.CustomerUpdateRole(userRoles);

                }
                else
                {
                    _cusRepo.CustomerRemoveRole(int_custid, "2");
                }

            }

            return RedirectToAction("CustomerIndex", "BackStage");
        }

        public ActionResult OrderDetails()
        {
            return View();
        }

        public ActionResult OrderEdit(string id)
        {
            var orderStatus = _orderRepo.GetOrderStatus(id);
            return View(orderStatus);
        }

        public ActionResult OrderIndex()
        {
            var getOrders = _orderRepo.GetAllOrders();
            List<Order> orders = new List<Order>();
            orders = getOrders.ToList();
            return View(orders);
        }

        public ActionResult ProductCreate()
        {
            return View();
        }

        public ActionResult ProductDelete()
        {
            return View();
        }

        public ActionResult ProductEdit()
        {
            return View();
        }

        public ActionResult ProductDetails()
        {
            return View();
        }

        public ActionResult ProductIndex()
        {
            return View();
        }

        


    }
}