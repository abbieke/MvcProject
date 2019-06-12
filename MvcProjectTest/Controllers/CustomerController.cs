using MvcProjectTest.Models;
using MvcProjectTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcProjectTest.Services;

namespace MvcProjectTest.Controllers
{
    public class CustomerController : Controller
    {
        public readonly CustomersRepository _repo;
        public CustomerController()
        {
            _repo = new CustomersRepository();
        }
        // GET: Customer
        public ActionResult Index()
        {
            var cust = _repo.ReadAllCustomer().ToList();
            return View(cust);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CustomerViewModel cust)
        {
            CustomerViewModel customer = new CustomerViewModel
            {
                CustomerName = cust.CustomerName,
                CustomerAddress = cust.CustomerAddress,
                CustomerAccount = cust.CustomerAccount,
                CustomerEmail = cust.CustomerEmail,
                CustomerBirth = cust.CustomerBirth,
                CustomerPhone = cust.CustomerPhone
            };
            _repo.UpdateCustomer(customer);
            return RedirectToAction("Index","Home");
        }

        public ActionResult CustomerIndex(CustomerMix cust, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var custom = _repo.SelectCustomerView(User.Identity.Name);
                var custpass = _repo.SelectCustomerPasswordView(User.Identity.Name);
                var custorder = _repo.SelectOrders(User.Identity.Name);
                CustomerMix mix = new CustomerMix() { CustomerView = custom, ChangePassword = custpass , Order = custorder};
                return View(mix);
            }
            if (id == "1")
            {
                CustomerMix customer = new CustomerMix
                {
                    CustomerView = cust.CustomerView
                };
                _repo.UpdateCustomer(customer.CustomerView);
                return RedirectToAction("Index", "Home");
            }
            else if (id == "2")
            {
                CustomerMix customer = new CustomerMix
                {
                    ChangePassword = cust.ChangePassword
                };
                if (HashService.SHA256Hash(customer.ChangePassword.CustomerPassword) == _repo.SelectCustomerPassword(customer.ChangePassword.CustomerAccount))
                {
                    _repo.UpdatePassword(customer.ChangePassword.CustomerAccount, customer.ChangePassword.CustomerNewPassword);
                    return RedirectToAction("CustomerIndex", "Customer", new { id = string.Empty });
                }
                else
                {
                    return Content("密碼錯誤");
                }
            }
            return RedirectToAction("CustomerIndex");
        }

        public ActionResult OrderDetail(int? id)
        {
            var order = _repo.SelectOrder(id);
            var orderDetail = _repo.SelectOrderDetails(id);
            var orderStatus = _repo.SelectOrderStatus(id);

            OrderDetailMix mix = new OrderDetailMix(){ Order = order, OrderDetails = orderDetail, OrderStatus = orderStatus};
            return View(mix);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CustomerIndex(CustomerMix cust)
        //{
        //    CustomerMix customer = new CustomerMix
        //    {
        //        CustomerView = cust.CustomerView
        //    };
        //    _repo.UpdateCustomer(customer.CustomerView);
        //    return RedirectToAction("Index", "Home");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CustomerIndex(CustomerMix cust,string id)
        //{
        //    if (id == "1")
        //    {
        //        CustomerMix customer = new CustomerMix
        //        {
        //            CustomerView = cust.CustomerView
        //        };
        //        _repo.UpdateCustomer(customer.CustomerView);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else if (id == "2")
        //    {
        //        CustomerMix customer = new CustomerMix
        //        {
        //            ChangePassword = cust.ChangePassword
        //        };
        //        if (customer.ChangePassword.CustomerPassword == _repo.SelectCustomerPassword(customer.ChangePassword.CustomerAccount))
        //        {
        //            _repo.UpdatePassword(customer.ChangePassword.CustomerAccount, customer.ChangePassword.CustomerNewPassword);
        //            return RedirectToAction("CustomerIndex");
        //        }
        //        else
        //        {
        //            return Content("密碼錯誤");
        //        }
        //    }
        //    return RedirectToAction("CustomerIndex");
        //}
    }
}