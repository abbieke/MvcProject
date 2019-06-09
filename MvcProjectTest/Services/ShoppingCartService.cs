using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectTest.ViewModels;
using MvcProjectTest.Models;
using MvcProjectTest.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MvcProjectTest.Services
{
    public class ShoppingCartService
    {
        private ShoppingRepository _cartRepo = new ShoppingRepository();
        private BooksRepository _bookRepo = new BooksRepository();
        private CustomersRepository _cusRepo = new CustomersRepository();
        public IEnumerable<ShoppingCartViewModel> GetMemberCart(string account)
        {
            var model = new List<ShoppingCartViewModel>();
            

            foreach(var item in _cartRepo.GetMemberCart(account))
            {
                var book = _bookRepo.GetBookById(item.BookID);
                var target = new ShoppingCartViewModel
                {
                    CartID = item.ShoppingCarID,
                    BookID = item.BookID,
                    BooksName = book.BooksName,
                    Discount = book.Discount,
                    InStock = book.InStock,
                    Quantity = item.Quantity,
                    BookImage = "Assets/Images/" + _bookRepo.GetCategoryEngNameById(book.CategoryID) + "/" + book.BookImage,
                    UnitPrice = book.UnitPrice
                };
                model.Add(target);
            }
            
            return model;
        }

        public void DeleteCartByAccount(string account)
        {

            if(account != null)
            {
                _cartRepo.DeleteByAccount(account);
            }
            else
            {
                throw new Exception("沒有成功取得用戶帳號名稱，請檢查");
            }
            
        }

        public bool ProductsAreOutOfStock(string cusAccount, IEnumerable<ShoppingCartViewModel> orderProducts)
        {
            
            foreach (var product in orderProducts)
            {
                if (_bookRepo.SelectBook(product.BookID).InStock < product.Quantity)
                {
                    return true;
                }
            }
            return false;
        }

        
        //驗證是否所有商品都在購物車有此項目
        public bool ProductlistIsNotCorrect(string cusAccount, IEnumerable<ShoppingCartViewModel> orderProducts)
        {
            
            var sqlCartList = _cartRepo.GetMemberCart(cusAccount);
            foreach(var product in orderProducts)
            {
                if((sqlCartList.FirstOrDefault((x)=>x.BookID == product.BookID))==null)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetErrorText(Error error)
        {
            switch (error)
            {
                case Error.accountError:
                    return "會員資料認證錯誤，請重試";
                case Error.emptyError:
                    return "您並未選擇任何商品。";
                case Error.inStockError:
                    return "商品庫存不足，有可能在購買過程中庫存有所更新，請重試";
                case Error.cartError:
                    return "訂單商品與購物車不相符，請重試";
                case Error.clearArgumentError:
                    return "清除購物車參數錯誤，請重試，如再次發生此錯誤請聯絡管理人員";
                case Error.otherError:
                    return "意外錯誤，請重試";
                
            }
            return "錯誤資訊未標明，請重試";
        }

        
        public enum Error
        {
            accountError, emptyError, inStockError, cartError, clearArgumentError, otherError
        }


    }
}