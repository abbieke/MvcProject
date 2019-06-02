﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProjectTest.Repositories;

namespace MvcProjectTest.Controllers
{
    

    [Authorize]
    public class ShoppingController : Controller
    {
        public readonly ShoppingRepository _repo;
        public readonly CustomersRepository _cusRepo;
        public ShoppingController()
        {
            _repo = new ShoppingRepository();
            _cusRepo = new CustomersRepository();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var carts = _repo.SelectCart(_cusRepo.GetCusromerID(User.Identity.Name));

                return View(carts);
            }
            
            return Redirect("/Account/Login");

        }
        public ActionResult ShippingInfo()
        {
            return View(); 
        }
        public ActionResult OrderCheck()
        {
            return View(); 
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }


        public void RemoveCartItem(int customerId, string bookId)
        {
            _repo.RemoveCartBook(customerId, bookId);
        }
       
        [HttpPost]
        public void AddToCart(string bookId)
        {

            int userid = (int)Session["userid"];
            _repo.InsertCartBook(userid, bookId);
            
        }
        
    }
}