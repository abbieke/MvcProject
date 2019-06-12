﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public BackStageController()
        {
            _cusRepo = new CustomersRepository();
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
            var CustDetail = _cusRepo.SelectCustomerDetail(id);
            return View(CustDetail);
        }

        public ActionResult CustomerIndex()
        {
            var Customers = _cusRepo.ReadAllCustomer();
            List<Customer> cust = new List<Customer>();
            cust = Customers.ToList();
            return View(cust);
        }

        public ActionResult OrderDetails()
        {
            return View();
        }

        public ActionResult OrderEdit()
        {
            return View();
        }

        public ActionResult OrderIndex()
        {
            return View();
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