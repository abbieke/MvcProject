using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProjectTest.Repositories;
using MvcProjectTest.Services;
using MvcProjectTest.ViewModels;

namespace MvcProjectTest.Controllers
{
    

    //[Authorize]
    public class ShoppingController : Controller
    {
        public readonly ShoppingRepository _repo;
        public readonly CustomersRepository _cusRepo;
        private readonly ShoppingCartService cartSer;
        public ShoppingController()
        {
            _repo = new ShoppingRepository();
            _cusRepo = new CustomersRepository();
            cartSer = new ShoppingCartService();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                //var carts = _repo.SelectCart(_cusRepo.GetCusromerID(User.Identity.Name));
                //var cartSer = new ShoppingCartService();
                var cartModel = cartSer.GetMemberCart(User.Identity.Name);
                

                return View(cartModel);
                
            }
            
                //TempData["PageFrom"] = Url.Action("Index", "Shopping");
            
            TempData["Message"] = "請先登入會員";
            return Redirect("/Account/Login");

        }
        public ActionResult ShippingInfo(string cusAccount, [Bind (Include = "BookID, Quantity")]IEnumerable<ShoppingCartViewModel> orderProducts, bool isNeedingClear)
        {
            var a = orderProducts;

            if (isNeedingClear)
            {
                cartSer.DeleteCartByAccount(cusAccount);
            }
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
        public string AddToCart(int quantity, string bookId)
        {
            if (Request.IsAuthenticated)
            {
                int userid = (int)Session["userid"];
                _repo.InsertCartBook(userid, bookId, quantity);
                return "success"+ quantity;
            }
            return "fail";

        }

        [HttpPost]
        public void DeleteCartFromAccount(string account)
        {
            cartSer.DeleteCartByAccount(account);
        }

        

        //1g的wish


        [HttpPost]
        public string AddToWish(string bookId)
        {
            if (Request.IsAuthenticated)
            {
                int userid = (int)Session["userid"];
                _repo.InsertWish(userid, bookId);
                return "success";
            }
            return "fail";

        }

        [HttpPost]
        public string RemoveFromWish(string bookId)
        {
            if (Request.IsAuthenticated)
            {
                int userid = (int)Session["userid"];
                _repo.RemoveWish(userid, bookId);
                return "success";
            }
            return "fail";

        }
        [HttpPost]
        public bool isInWish(string bookId)
        {
            if (Request.IsAuthenticated)
            {
                int userid = (int)Session["userid"];
                return _repo.SelectWish(userid, bookId);
                             
            }
            else {
                return false;
            }

        }
        
        [HttpPost]
        public ActionResult HeadCartView()
        {
            if (Request.IsAuthenticated)
            {
                var cartSer = new ShoppingCartService();
                var cartModel = cartSer.GetMemberCart(User.Identity.Name);
                
                return PartialView("_CartHoverPartial", cartModel);
                // return View(cartModel);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}