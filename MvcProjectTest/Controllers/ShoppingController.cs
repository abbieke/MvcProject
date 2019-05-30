using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProjectTest.Repositories;

namespace MvcProjectTest.Controllers
{
    

    //[Authorize]
    public class ShoppingController : Controller
    {
        public readonly ShoppingRepository _repo;
        public ShoppingController()
        {
            _repo = new ShoppingRepository();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            var books=_repo.SelectCart(4);

            return View(books); 
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
        
    }
}