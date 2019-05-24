using MvcProjectTest.Models;
using MvcProjectTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            var cust = _repo.SelectCustomerView(User.Identity.Name);
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

        public ActionResult CustomerIndex()
        {
            var cust = _repo.SelectCustomerView(User.Identity.Name);
            return View(cust);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerIndex(CustomerViewModel cust)
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
            return RedirectToAction("Index", "Home");
        }
    }
}