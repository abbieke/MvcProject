using System;
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

namespace MvcProjectTest.Controllers


{
    

    //[Authorize]
    public class ShoppingController : JsonNetController
    {
        private readonly ShoppingRepository _repo;
        private readonly CustomersRepository _cusRepo;
        private readonly ShoppingCartService _cartSer;
        
        public ShoppingController()
        {
            _repo = new ShoppingRepository();
            _cusRepo = new CustomersRepository();
            _cartSer = new ShoppingCartService();
            
        }
        // GET: Shopping
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                
                //var carts = _repo.SelectCart(_cusRepo.GetCusromerID(User.Identity.Name));
                //var cartSer = new ShoppingCartService();
                var cartModel = _cartSer.GetMemberCart(User.Identity.Name);
                
                //if (TempData.Keys.Contains("message") && TempData["message"] != null)
                //{
                //    ViewBag.isWarning = "True";
                //    ViewBag.warningText = TempData["message"];
                //}
                //else
                //{
                //    ViewBag.isWarning = "False";
                //}

                return View(cartModel);
                
            }
            
                //TempData["PageFrom"] = Url.Action("Index", "Shopping");
            
            TempData["Message"] = "請先登入會員";
            return Redirect("/Account/Login");

        }
        //備忘錄：目前拿不到errortext 雖然不需要 但有空時可以找找原因
        public ActionResult ShippingInfo(string cusAccount, [Bind (Include = "BookID, Quantity")]IEnumerable<ShoppingCartViewModel> orderProducts, bool isNeedingClear)
        {
            //因為是藉由網址導向此頁，再轉向後進行再次驗證
            var a = CheckCartResult(cusAccount, orderProducts, isNeedingClear);
            var b = JsonConvert.SerializeObject(a);
            var errorModel = JsonConvert.DeserializeObject<CartErrorModel>(b);
            if (errorModel.IsError)
            {
                throw new Exception("在驗證後傳送資訊可能遭到變更，請確認");
            }

            

            if (isNeedingClear)
            {
                _cartSer.DeleteCartByAccount(cusAccount);
            }
            return View(orderProducts);
        }
        public ActionResult OrderCheck()
        {
            return View(); 
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }


        
        //覆寫過新增的result
        public JsonNetResult CheckCartResult(string cusAccount, [Bind(Include = "BookID, Quantity")]IEnumerable<ShoppingCartViewModel> orderProducts, bool? isNeedingClear)
        {
            var a = orderProducts;
            CartErrorModel checkResult = new CartErrorModel();
            //驗證帳戶
            if(cusAccount == null || cusAccount!=User.Identity.Name || _cusRepo.GetCusromerID(cusAccount)==0 )
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.accountError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.accountError);
                return new JsonNetResult { Data = checkResult };
            }
            //驗證項目是否對應用戶購物車
            if(_cartSer.ProductlistIsNotCorrect(cusAccount, orderProducts))
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.cartError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.cartError);
                return new JsonNetResult { Data = checkResult };
            }
            //驗證商品庫存是否足夠
            if(_cartSer.ProductsAreOutOfStock(cusAccount, orderProducts))
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.inStockError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.inStockError);
                return new JsonNetResult { Data = checkResult };
            }
            //清除鈕系統防呆，單純確認所有參數正確性，若發生這個錯誤要檢查購物車View
            if (isNeedingClear == null)
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.clearArgumentError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.clearArgumentError);
                return new JsonNetResult { Data = checkResult };
            }



            checkResult.IsError = false;
            return new JsonNetResult { Data = checkResult };

        }




        [Route("/Shopping/ErrorPage/{error}")]
        public ActionResult ErrorPage(ShoppingCartService.Error error)
        {
            var errorModel = new CartErrorModel();
            errorModel.ErrorText = ShoppingCartService.GetErrorText(error);
            return View(errorModel);
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
            _cartSer.DeleteCartByAccount(account);
        }

        
        

        //下面是1g的wish


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