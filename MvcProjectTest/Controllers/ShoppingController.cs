using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProjectTest.Repositories;
using MvcProjectTest.Services;

namespace MvcProjectTest.Controllers
{
    

    //[Authorize]
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
                //var carts = _repo.SelectCart(_cusRepo.GetCusromerID(User.Identity.Name));
                var cartSer = new ShoppingCartService();
                var cartModel = cartSer.GetMemberCart(User.Identity.Name);


                return View(cartModel);
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
        
    }
}