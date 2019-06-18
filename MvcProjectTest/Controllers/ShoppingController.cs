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
using MvcProjectTest.Models;

namespace MvcProjectTest.Controllers


{
    

    //[Authorize]
    public class ShoppingController : JsonNetController
    {
        private readonly ShoppingRepository _repo;
        private readonly CustomersRepository _cusRepo;
        private readonly BooksRepository _bookRepo;
        private readonly ShoppingCartService _cartSer;
        private readonly OrderService _orderSer;
        private readonly OrderRepository _orderRepo;
        private readonly ShoppingRepository _cartRepo;

        //先頂著用
        private static Order _order;
        private static IEnumerable<ShoppingCartViewModel> opList;
        private static bool _isNeedingClear;


        public ShoppingController()
        {
            _repo = new ShoppingRepository();
            _cusRepo = new CustomersRepository();
            _bookRepo = new BooksRepository();
            _cartSer = new ShoppingCartService();
            _orderSer = new OrderService();
            _orderRepo = new OrderRepository();
            _cartRepo = new ShoppingRepository();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("NotVerified"))
                {
                    return RedirectToAction("CustomerIndex", "Customer");
                }
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
        [HttpPost]
        public ActionResult ShippingInfo(string cusAccount, [Bind (Include = "BookID, Quantity")]IEnumerable<ShoppingCartViewModel> orderProducts, bool isNeedingClear)
        {
            if (isNeedingClear)
            {
                _isNeedingClear = true;
            }

            
            return View(opList);
        }
        public ActionResult OrderCheck(OrderService.PayWay payWay, OrderService.DeliveryMethod deliveryMethod, string name, string phone, string email, string address)
        {
            int shippingRate = 60;

            Order orderModel = new Order();
            orderModel.PayWay = payWay.ToString();
            orderModel.DeliveryMethod = deliveryMethod.ToString();
            orderModel.Recipient = name;
            orderModel.RecipientPhone = phone;
            orderModel.RecipientEmail = email;
            orderModel.RecipientAddress = address;

            orderModel.OrderNo = _orderSer.GetFakeOrderNo();
            orderModel.CustomerID = _cusRepo.GetCusromerID(User.Identity.Name);
            orderModel.ShippingRate = shippingRate;

            orderModel.OrderDate = DateTime.Now;

            foreach(var item in opList)
            {
                var book = _bookRepo.GetBookById(item.BookID);
                item.UnitPrice = book.UnitPrice;
                item.Discount = book.Discount;
                item.BookImage = "/Assets/Images/" + _bookRepo.GetCategoryEngNameById(book.CategoryID) + "/" + book.BookImage;
                item.BooksName = book.BooksName;
            }

            orderModel.TotalPrice = 
                opList.Sum((x) => Math.Round(x.UnitPrice * x.Quantity * (1 - x.Discount))) + shippingRate;



            _order = orderModel;
            return View(orderModel); 
        }
        public async System.Threading.Tasks.Task<ActionResult> OrderSuccess()
        {
            //驗證商品清單
            var a = CheckCartResult(User.Identity.Name, opList, false);
            var b = JsonConvert.SerializeObject(a);
            var errorModel = JsonConvert.DeserializeObject<CartErrorModel>(b);
            if (errorModel.IsError)
            {
                return Redirect("/Shopping/ErrorPage/" + errorModel.ErrorType.ToString());
                //throw new Exception("在驗證後傳送資訊可能遭到變更，請確認");
            }


            _order.SetUp = DateTime.Now;
            //加訂單請寫在註解中間
            Order order;
            _orderRepo.CreateOrder(_order);
            order=_orderRepo.GetOrderFromOrderNo(_order.OrderNo);

            _orderRepo.CreateOrderStatus(order.OrderID,_order);

            _orderRepo.CreateOrderDetail(order.OrderID, opList);

            foreach(var item in opList)
            {
                _cartRepo.RemoveCartBook(_cusRepo.GetCusromerID(User.Identity.Name), item.BookID);
            }


            //

            Order orderModel = _order;
            string cust_mail = _cusRepo.SelectCustomerEmail((int)Session["userid"]);
            await MailService.SendMailToNoticeOrderSuccess(cust_mail, _order.OrderNo);
            if (_isNeedingClear)
            {
                _cartSer.DeleteCartByAccount(User.Identity.Name);
            }
            _isNeedingClear = false;
            _order = null;
            return View(orderModel);
        }

        public JsonResult GetOpList()
        {
            return Json(opList, JsonRequestBehavior.AllowGet);
        }


        
        //覆寫過新增的result
        public JsonNetResult CheckCartResult(string cusAccount, [Bind(Include = "BookID, Quantity")]IEnumerable<ShoppingCartViewModel> orderProducts, bool? isNeedingClear)
        {
            
            CartErrorModel checkResult = new CartErrorModel();
            opList = null;

            //驗證帳戶
            if(cusAccount == null || cusAccount!=User.Identity.Name || _cusRepo.GetCusromerID(cusAccount)==0 )
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.accountError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.accountError);
                return new JsonNetResult { Data = checkResult };
            }
            //確認購物車是否選中商品為空，要排前面
            if (orderProducts == null || orderProducts.Count()==0)
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.emptyError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.emptyError);
                return new JsonNetResult { Data = checkResult };
            }
            //確認是否有為0的商品
            if (orderProducts.Any((x)=>x.Quantity == 0))
            {
                checkResult.IsError = true;
                checkResult.ErrorType = ShoppingCartService.Error.zeroError;
                checkResult.ErrorText = ShoppingCartService.GetErrorText(ShoppingCartService.Error.zeroError);
                return new JsonNetResult { Data = checkResult };
            }
            //驗證項目是否對應用戶購物車
            if (_cartSer.ProductlistIsNotCorrect(cusAccount, orderProducts))
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

            //先這樣用一下..
            opList = orderProducts;

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